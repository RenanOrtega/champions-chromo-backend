using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.DeleteSchool
{
    public record DeleteSchoolCommand(string Id) : IRequest<Result>;
}
