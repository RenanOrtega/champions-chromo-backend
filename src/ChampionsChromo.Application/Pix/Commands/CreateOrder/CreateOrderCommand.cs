using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Entities.Pix;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Result<GeneratePixAbacatePayResponse>>
{
    public Payment Payment { get; set; }
    public Address Address { get; set; }
    public Customer Customer { get; set; }
    public int ExpiresIn { get; set; }
    public string Description { get; set; }
}