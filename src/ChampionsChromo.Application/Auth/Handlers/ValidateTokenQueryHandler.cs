using ChampionsChromo.Application.Auth.Queries;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class ValidateTokenQueryHandler(IAuthService authService) : IRequestHandler<ValidateTokenQuery, Result<bool>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<bool>> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        var isValid = await _authService.ValidateTokenAsync(request.Token);
        return Result<bool>.Success(isValid);
    }
}
