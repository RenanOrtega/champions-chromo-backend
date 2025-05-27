using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Auth.Commands;

public record RefreshCommand(string RefreshToken) : IRequest<Result>;
