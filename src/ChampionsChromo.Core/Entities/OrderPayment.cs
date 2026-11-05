using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Entities;

public class OrderPayment
{
    public PaymentStatus Status { get; set; } = PaymentStatus.NotStarted;
    public string Provider { get; set; } = "mercadopago";
    public string Method { get; set; } = "pix";

    // Bumped every time a new Pix is generated for the same order (used in the idempotency key).
    public int Attempt { get; set; }

    public string? ProviderOrderId { get; set; }
    public string? ProviderPaymentId { get; set; }
    public string? ProviderStatus { get; set; }
    public string? ProviderStatusDetail { get; set; }

    public string? QrCode { get; set; }
    public string? QrCodeBase64 { get; set; }
    public string? TicketUrl { get; set; }

    public decimal Amount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? LastCheckedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
