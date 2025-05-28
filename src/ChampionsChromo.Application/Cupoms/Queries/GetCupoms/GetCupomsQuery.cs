using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Queries.GetCupoms;

public record GetCupomsQuery : IRequest<Result<IEnumerable<CupomDto>>>;
