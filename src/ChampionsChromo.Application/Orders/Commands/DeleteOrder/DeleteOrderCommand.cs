using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(string Id) : IRequest<Result>;
}
