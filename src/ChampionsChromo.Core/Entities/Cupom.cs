using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Entities;

public class Cupom : Entity
{
    public string Code { get; set; }
    public CupomType Type { get; set; }
    public int Value { get; set; }
    public int UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int MinPurchaseValue { get; set; }
    public bool IsActive { get; set; }
}
