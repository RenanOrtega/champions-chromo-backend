using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await _authService.LoginAsync(request.Username, request.Password);
        return Result.Success();
    }
}