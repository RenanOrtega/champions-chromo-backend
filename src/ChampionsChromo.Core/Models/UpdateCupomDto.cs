using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Models;

public class UpdateCupomDto
{
    public CupomType Type { get; set; }
    public int Value { get; set; }
    public int UsageLimit { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int MinPurchaseValue { get; set; }
    public bool IsActive { get; set; }
}
