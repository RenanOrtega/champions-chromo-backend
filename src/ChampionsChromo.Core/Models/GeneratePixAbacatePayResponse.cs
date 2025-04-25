using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Models;

public class GeneratePixAbacatePayResponse
{
    public GeneratePixAbacatePayResponseData? Data { get; set; }
    public string? Error { get; set; } = string.Empty;
}

public class GeneratePixAbacatePayResponseData
{
    public required string Id { get; set; }
    public int Amount { get; set; }
    public PixStatus Status { get; set; }
    public bool DevMode { get; set; }
    public string BrCode { get; set; } = string.Empty;
    public string BrCodeBase64 { get; set; } = string.Empty;
    public int PlatformFee { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}