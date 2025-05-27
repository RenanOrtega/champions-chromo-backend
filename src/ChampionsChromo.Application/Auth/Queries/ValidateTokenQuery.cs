using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Auth.Queries;

public record ValidateTokenQuery(string Token) : IRequest<Result<bool>>;
