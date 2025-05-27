using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result<AuthResult>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.LoginAsync(request.Username, request.Password);

        if (!authResult.Success)
            return Result<AuthResult>.Failure(authResult.Message);

        return Result<AuthResult>.Success(authResult);
    }
}