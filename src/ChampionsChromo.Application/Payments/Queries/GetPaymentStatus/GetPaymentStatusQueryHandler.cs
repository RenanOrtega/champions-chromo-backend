using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Core.Services;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ChampionsChromo.Application.Payments.Queries.GetPaymentStatus;

public class GetPaymentStatusQueryHandler(
    IOrderRepository orderRepository,
    IPaymentGateway paymentGateway,
    ILogger<GetPaymentStatusQueryHandler> logger) : IRequestHandler<GetPaymentStatusQuery, Result<PixPaymentDto>>
{
    // The webhook is the main source of truth. While the customer waits on the Pix screen we also
    // ask Mercado Pago directly, at most once per interval, in case a notification is delayed or lost.
    private static readonly TimeSpan SyncInterval = TimeSpan.FromSeconds(20);

    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPaymentGateway _paymentGateway = paymentGateway;
    private readonly ILogger<GetPaymentStatusQueryHandler> _logger = logger;

    public async Task<Result<PixPaymentDto>> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(request.OrderId, out _))
            return Result<PixPaymentDto>.Failure("Pedido não encontrado.");

        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order is null)
            return Result<PixPaymentDto>.Failure("Pedido não encontrado.");

        var payment = order.Payment;
        var now = DateTime.UtcNow;

        if (payment?.Status == PaymentStatus.AwaitingPayment
            && !string.IsNullOrEmpty(payment.ProviderOrderId)
            && (payment.LastCheckedAt is null || now - payment.LastCheckedAt.Value >= SyncInterval))
        {
            try
            {
                var gatewayOrder = await _paymentGateway.GetOrderAsync(payment.ProviderOrderId, cancellationToken);
                if (gatewayOrder is not null)
                {
                    var updated = PaymentSync.Apply(order, gatewayOrder, now, out _);
                    if (updated is not null)
                    {
                        var statusChange = PaymentSync.StatusChangeFor(order, updated.Status, now, "mercadopago");
                        order.Payment = updated;
                        await _orderRepository.SetPaymentAsync(order.Id, updated, statusChange);
                    }
                }
            }
            catch (PaymentGatewayException ex)
            {
                _logger.LogWarning(ex, "Could not refresh payment of order {OrderId}", order.Id);
            }
        }

        return Result<PixPaymentDto>.Success(PaymentSync.ToDto(order));
    }
}
