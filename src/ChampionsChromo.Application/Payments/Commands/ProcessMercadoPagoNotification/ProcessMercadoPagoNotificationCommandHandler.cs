using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ChampionsChromo.Application.Payments.Commands.ProcessMercadoPagoNotification;

public class ProcessMercadoPagoNotificationCommandHandler(
    IOrderRepository orderRepository,
    IPaymentGateway paymentGateway,
    ILogger<ProcessMercadoPagoNotificationCommandHandler> logger) : IRequestHandler<ProcessMercadoPagoNotificationCommand, Result>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPaymentGateway _paymentGateway = paymentGateway;
    private readonly ILogger<ProcessMercadoPagoNotificationCommandHandler> _logger = logger;

    public async Task<Result> Handle(ProcessMercadoPagoNotificationCommand request, CancellationToken cancellationToken)
    {
        // Gateway errors propagate so the webhook answers 5xx and Mercado Pago retries later.
        var gatewayOrder = await _paymentGateway.GetOrderAsync(request.ProviderOrderId, cancellationToken);
        if (gatewayOrder is null)
        {
            _logger.LogWarning("Mercado Pago notification for unknown order {ProviderOrderId}", request.ProviderOrderId);
            return Result.Success();
        }

        var orderId = gatewayOrder.ExternalReference;
        if (string.IsNullOrEmpty(orderId) || !ObjectId.TryParse(orderId, out _))
        {
            _logger.LogWarning("Mercado Pago order {ProviderOrderId} has no valid external_reference", gatewayOrder.Id);
            return Result.Success();
        }

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order is null)
        {
            _logger.LogWarning("Mercado Pago order {ProviderOrderId} points to missing order {OrderId}", gatewayOrder.Id, orderId);
            return Result.Success();
        }

        var updated = PaymentSync.Apply(order, gatewayOrder, DateTime.UtcNow, out var rejection);
        if (rejection is not null)
            _logger.LogError("Order {OrderId} not marked as paid: {Reason}", orderId, rejection);

        if (updated is not null)
        {
            var statusChange = PaymentSync.StatusChangeFor(order, updated.Status, DateTime.UtcNow, "mercadopago");
            await _orderRepository.SetPaymentAsync(order.Id, updated, statusChange);
            _logger.LogInformation("Order {OrderId} payment is now {Status} (Mercado Pago {ProviderOrderId}: {ProviderStatus}/{Detail})",
                order.Id, updated.Status, gatewayOrder.Id, gatewayOrder.Status, gatewayOrder.StatusDetail);
        }

        return Result.Success();
    }
}
