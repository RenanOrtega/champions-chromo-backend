using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Entities.Pix;
using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Result>
{
    public string IntegrationId { get; set; }
    public Payment Payment { get; set; }
    public Address Address { get; set; }
    public Customer Customer { get; set; }
    public PixStatus Status { get; set; }
}