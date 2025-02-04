using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Schools.Queries.GetSchoolById;

public record GetSchoolByIdQuery(string Id) : IRequest<Result<SchoolDto>>;
