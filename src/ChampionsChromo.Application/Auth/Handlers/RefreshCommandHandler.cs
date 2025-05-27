using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class RefreshCommandHandler(IAuthService authService) : IRequestHandler<RefreshCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        await _authService.RefreshTokenAsync(request.RefreshToken);
        return Result.Success();
    }
}