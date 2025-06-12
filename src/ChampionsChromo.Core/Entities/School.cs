namespace ChampionsChromo.Core.Entities;

public class School : Entity
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Warning { get; set; } = string.Empty;
    public string BgWarningColor { get; set; } = string.Empty;
    public int ShippingCost { get; set; }
}