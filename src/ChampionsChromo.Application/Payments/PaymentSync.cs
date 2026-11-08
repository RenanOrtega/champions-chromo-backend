using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Services.Interfaces;

namespace ChampionsChromo.Application.Payments;

public static class PaymentSync
{
    // Mercado Pago Orders API order statuses -> our status.
    public static PaymentStatus FromProvider(string providerStatus) => providerStatus switch
    {
        "processed" => PaymentStatus.Paid,
        "created" or "processing" or "action_required" => PaymentStatus.AwaitingPayment,
        "expired" => PaymentStatus.Expired,
        "canceled" or "cancelled" or "failed" => PaymentStatus.Cancelled,
        "refunded" or "charged_back" => PaymentStatus.Refunded,
        _ => PaymentStatus.AwaitingPayment
    };

    /// <summary>
    /// Applies what Mercado Pago says about one of its orders to our order.
    /// Returns the new payment to persist, or null when nothing should change.
    /// </summary>
    public static OrderPayment? Apply(OrderSummary order, GatewayOrder gatewayOrder, DateTime now, out string? rejection)
    {
        rejection = null;
        var current = order.Payment ?? new OrderPayment();
        var newStatus = FromProvider(gatewayOrder.Status);

        // Never let anything downgrade a paid order except a refund.
        if (current.Status == PaymentStatus.Paid && newStatus != PaymentStatus.Refunded)
            return null;

        var isCurrentAttempt = string.Equals(current.ProviderOrderId, gatewayOrder.Id, StringComparison.OrdinalIgnoreCase);

        if (newStatus == PaymentStatus.Paid)
        {
            if (gatewayOrder.TotalAmount < order.PriceTotal)
            {
                rejection = $"Valor pago ({gatewayOrder.TotalAmount}) menor que o total do pedido ({order.PriceTotal}).";
                return null;
            }
        }
        else if (!isCurrentAttempt)
        {
            // An old Pix of this order expired/was cancelled: irrelevant, a newer one is active.
            return null;
        }

        if (isCurrentAttempt && current.Status == newStatus && current.ProviderStatusDetail == gatewayOrder.StatusDetail)
        {
            current.LastCheckedAt = now;
            return current;
        }

        current.Status = newStatus;
        current.ProviderOrderId = gatewayOrder.Id;
        current.ProviderPaymentId = gatewayOrder.PaymentId ?? current.ProviderPaymentId;
        current.ProviderStatus = gatewayOrder.Status;
        current.ProviderStatusDetail = gatewayOrder.StatusDetail;
        current.LastCheckedAt = now;
        current.UpdatedAt = now;

        if (newStatus == PaymentStatus.Paid)
            current.PaidAt ??= now;

        return current;
    }

    /// <summary>
    /// How a payment status moves the order status. Returns null when the order should stay as it is
    /// (for example, an order already being prepared or shipped is not moved back by a late notification).
    /// </summary>
    public static OrderStatusChange? StatusChangeFor(OrderSummary order, PaymentStatus paymentStatus, DateTime now, string by)
    {
        OrderStatus? next = paymentStatus switch
        {
            PaymentStatus.Paid when order.Status is OrderStatus.New or OrderStatus.AwaitingPayment
                or OrderStatus.Expired or OrderStatus.Cancelled => OrderStatus.Paid,
            PaymentStatus.AwaitingPayment when order.Status is OrderStatus.New or OrderStatus.Expired => OrderStatus.AwaitingPayment,
            PaymentStatus.Expired when order.Status is OrderStatus.AwaitingPayment => OrderStatus.Expired,
            PaymentStatus.Cancelled when order.Status is OrderStatus.AwaitingPayment => OrderStatus.Cancelled,
            PaymentStatus.Refunded when order.Status is not OrderStatus.Refunded => OrderStatus.Refunded,
            _ => null
        };

        if (next is null || next == order.Status)
            return null;

        return new OrderStatusChange { Status = next.Value, At = now, By = by };
    }

    public static PixPaymentDto ToDto(OrderSummary order) => new()
    {
        OrderId = order.Id,
        Status = order.Payment?.Status ?? PaymentStatus.NotStarted,
        Amount = order.PriceTotal,
        QrCode = order.Payment?.Status == PaymentStatus.AwaitingPayment ? order.Payment.QrCode : null,
        QrCodeBase64 = order.Payment?.Status == PaymentStatus.AwaitingPayment ? order.Payment.QrCodeBase64 : null,
        TicketUrl = order.Payment?.Status == PaymentStatus.AwaitingPayment ? order.Payment.TicketUrl : null,
        ExpiresAt = order.Payment?.ExpiresAt,
        PaidAt = order.Payment?.PaidAt
    };
}
