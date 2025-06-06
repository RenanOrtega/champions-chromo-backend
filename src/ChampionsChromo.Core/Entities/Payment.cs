namespace ChampionsChromo.Core.Entities;

public class Payment : Entity
{
    public long Amount { get; set; }
    public string PaymentGatewayId { get; set; }
    public DateTime? CanceledAt { get; set; }
    public string? CancellationReason { get; set; }
    public string ClientSecret { get; set; }
    public string Currency { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public string Status { get; set; }
}
