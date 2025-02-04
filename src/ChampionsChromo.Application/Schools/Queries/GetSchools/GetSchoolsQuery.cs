using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Schools.Queries.GetSchools;

public record GetSchoolsQuery : IRequest<Result<IEnumerable<SchoolDto>>>;
