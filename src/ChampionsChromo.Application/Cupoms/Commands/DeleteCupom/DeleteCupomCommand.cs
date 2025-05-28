using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Commands.DeleteCupom
{
    public record DeleteCupomCommand(string Id) : IRequest<Result>;
}
