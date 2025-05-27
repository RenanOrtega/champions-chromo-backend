using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using MediatR;

namespace ChampionsChromo.Application.Auth.Queries;

public record GetCurrentUserQuery(string Token) : IRequest<Result<User?>>;