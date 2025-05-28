using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Queries.GetCupomById;

public record GetCupomByIdQuery(string Id) : IRequest<Result<CupomDto>>;
