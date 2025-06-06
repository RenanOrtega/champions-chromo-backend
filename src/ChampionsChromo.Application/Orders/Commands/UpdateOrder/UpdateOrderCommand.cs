using System.Text.Json.Serialization;
using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand : IRequest<Result>
{
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;
}
