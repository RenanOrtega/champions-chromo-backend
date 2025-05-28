using System.Text.Json.Serialization;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Commands.UpdateCupom;

public record UpdateCupomCommand : IRequest<Result>
{
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;
    public CupomType Type { get; set; }
    public int Value { get; set; }
    public int UsageLimit { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int MinPurchaseValue { get; set; }
    public bool IsActive { get; set; }
}
