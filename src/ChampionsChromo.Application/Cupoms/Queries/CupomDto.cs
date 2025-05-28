using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Application.Cupoms.Queries;

public class CupomDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; }
    public CupomType Type { get; set; }
    public int Value { get; set; }
    public int UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int MinPurchaseValue { get; set; }
    public bool IsActive { get; set; }
}
