namespace ChampionsChromo.Core.Enums;

public enum PaymentStatus
{
    // Orders created before the Mercado Pago integration (closed via WhatsApp) or with no Pix generated yet.
    NotStarted,
    AwaitingPayment,
    Paid,
    Expired,
    Cancelled,
    Refunded
}
