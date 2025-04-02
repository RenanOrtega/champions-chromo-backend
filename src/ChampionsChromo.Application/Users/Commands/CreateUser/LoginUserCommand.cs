using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Users.Commands.CreateUser;

public record LoginUserCommand : IRequest<Result<string>>
{
    public string GoogleId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}
