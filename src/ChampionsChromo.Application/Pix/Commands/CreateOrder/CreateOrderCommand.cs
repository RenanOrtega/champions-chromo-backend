using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Result<GeneratePixAbacatePayResponse>>
{
    public Address Address { get; set; }
    public Customer Customer { get; set; }
    public int ExpiresIn { get; set; }
    public string Description { get; set; }
}