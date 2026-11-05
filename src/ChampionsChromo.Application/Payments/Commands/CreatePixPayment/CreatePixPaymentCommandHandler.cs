using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Options;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Core.Services;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ChampionsChromo.Application.Payments.Commands.CreatePixPayment;

public class CreatePixPaymentCommandHandler(
    IOrderRepository orderRepository,
    IPaymentGateway paymentGateway,
    IOptions<MercadoPagoOptions> options) : IRequestHandler<CreatePixPaymentCommand, Result<PixPaymentDto>>
{
    // A Pix that expires in less than this is not handed out again; a new one is generated.
    private static readonly TimeSpan MinimumRemaining = TimeSpan.FromMinutes(2);

    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPaymentGateway _paymentGateway = paymentGateway;
    private readonly MercadoPagoOptions _options = options.Value;

    public async Task<Result<PixPaymentDto>> Handle(CreatePixPaymentCommand request, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(request.OrderId, out _))
            return Result<PixPaymentDto>.Failure("Pedido não encontrado.");

        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order is null)
            return Result<PixPaymentDto>.Failure("Pedido não encontrado.");

        var now = DateTime.UtcNow;
        var payment = order.Payment;

        if (payment?.Status is PaymentStatus.Paid or PaymentStatus.Refunded)
            return Result<PixPaymentDto>.Success(PaymentSync.ToDto(order));

        // Reuse the Pix already generated while it is still valid (page reloads, double clicks).
        if (payment?.Status == PaymentStatus.AwaitingPayment
            && payment.ExpiresAt is not null
            && payment.ExpiresAt.Value - now > MinimumRemaining)
        {
            return Result<PixPaymentDto>.Success(PaymentSync.ToDto(order));
        }

        if (order.PriceTotal <= 0)
            return Result<PixPaymentDto>.Failure("Pedido sem valor a pagar.");

        if (string.IsNullOrWhiteSpace(order.Customer?.Email))
            return Result<PixPaymentDto>.Failure("Pedido sem email do comprador.");

        var attempt = (payment?.Attempt ?? 0) + 1;
        var expiresIn = TimeSpan.FromMinutes(Math.Clamp(_options.PixExpirationMinutes, 30, 60 * 24));

        GatewayOrder created;
        try
        {
            created = await _paymentGateway.CreatePixOrderAsync(new CreatePixOrderRequest(
                ExternalReference: order.Id,
                Amount: order.PriceTotal,
                PayerEmail: order.Customer.Email,
                PayerFirstName: FirstName(order.Customer.Name),
                // Same key for the same attempt: concurrent requests get the same Pix back.
                IdempotencyKey: $"order-{order.Id}-pix-{attempt}",
                ExpiresIn: expiresIn), cancellationToken);
        }
        catch (PaymentGatewayException)
        {
            return Result<PixPaymentDto>.Failure("Não foi possível gerar o Pix agora. Tente novamente em instantes.");
        }

        order.Payment = new OrderPayment
        {
            Status = PaymentSync.FromProvider(created.Status),
            Attempt = attempt,
            ProviderOrderId = created.Id,
            ProviderPaymentId = created.PaymentId,
            ProviderStatus = created.Status,
            ProviderStatusDetail = created.StatusDetail,
            QrCode = created.QrCode,
            QrCodeBase64 = created.QrCodeBase64,
            TicketUrl = created.TicketUrl,
            Amount = order.PriceTotal,
            ExpiresAt = now.Add(expiresIn),
            PaidAt = PaymentSync.FromProvider(created.Status) == PaymentStatus.Paid ? now : null,
            LastCheckedAt = now,
            UpdatedAt = now
        };

        await _orderRepository.SetPaymentAsync(order.Id, order.Payment);

        return Result<PixPaymentDto>.Success(PaymentSync.ToDto(order));
    }

    private static string FirstName(string? fullName)
    {
        var first = fullName?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        return string.IsNullOrEmpty(first) ? "Cliente" : first[..Math.Min(first.Length, 50)];
    }
}
