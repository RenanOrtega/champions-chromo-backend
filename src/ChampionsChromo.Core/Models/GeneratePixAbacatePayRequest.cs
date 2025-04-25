using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Models;

public class GeneratePixAbacatePayRequest
{
    public int Amount { get; set; }
    public int ExpiresIn { get; set; }
    public string Description { get; set; } = string.Empty;
    public required Customer Customer { get; set; }
}
