using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Application.Payments;

// What the store needs to show the Pix screen. No customer data here: the order id is the only key.
public class PixPaymentDto
{
    public string OrderId { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string? QrCode { get; set; }
    public string? QrCodeBase64 { get; set; }
    public string? TicketUrl { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? PaidAt { get; set; }
}
