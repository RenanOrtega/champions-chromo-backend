using System.Text.Json.Serialization;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.UpdateOrderStatus;

// Admin changes the order status by hand (delivery steps, or payment in manual mode).
public class UpdateOrderStatusCommand : IRequest<Result>
{
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;

    [JsonIgnore]
    public string ChangedBy { get; set; } = "admin";

    public OrderStatus Status { get; set; }
    public string? Note { get; set; }
}
