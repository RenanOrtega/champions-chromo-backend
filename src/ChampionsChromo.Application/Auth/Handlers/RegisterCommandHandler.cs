using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommand, Result<AuthResult>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.RegisterAsync(request.Username, request.Password);
        return Result<AuthResult>.Success(authResult);
    }
}
