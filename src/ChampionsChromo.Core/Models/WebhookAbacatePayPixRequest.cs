using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Models;

public record WebhookAbacatePayPixRequest
{
    public required PixQrCodeData PixQrCode { get; set; }
    public string Event { get; set; } = string.Empty;
}

public class PixQrCodeData
{
    public string Id { get; set; } = string.Empty;
    public PixStatus Status { get; set; }
}
