using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Auth.Commands;

public record LoginCommand(string Username, string Password) : IRequest<Result<AuthResult>>;
