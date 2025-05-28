using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Commands.CreateCupom;

public record CreateCupomCommand : IRequest<Result>
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
