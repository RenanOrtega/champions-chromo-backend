namespace ChampionsChromo.Core.Entities;

public class PixStatus : Entity
{
    public int Amount { get; set; }
    public int Fee { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string PixId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
