namespace ChampionsChromo.Core.Enums;

// Order lifecycle shown in the admin. Payment statuses are set automatically by Mercado Pago
// (when Payments:Mode is MercadoPagoPix); delivery statuses are set by the admin.
public enum OrderStatus
{
    // Manual mode: order created, payment is arranged over WhatsApp. Also the value of orders created before statuses existed.
    New,
    AwaitingPayment,
    Paid,
    // "Feito": the team took the order and asked for the cards to be made (set by the admin).
    Made,
    // "Enviado": the cards were sent (set by the admin).
    Shipped,
    Expired,
    Cancelled,
    Refunded
}
