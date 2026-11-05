using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Auth.Commands;

public record LogoutCommand(string? RefreshToken) : IRequest<Result>;
