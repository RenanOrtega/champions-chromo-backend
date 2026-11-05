using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request.RefreshToken);
        return Result.Success();
    }
}
