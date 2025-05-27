using ChampionsChromo.Application.Auth.Queries;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Auth.Handlers;

public class GetCurrentUserQueryHandler(IAuthService authService) : IRequestHandler<GetCurrentUserQuery, Result<User?>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<User?>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _authService.GetCurrentUserAsync(request.Token);
        return Result<User?>.Success(user);
    }
}
