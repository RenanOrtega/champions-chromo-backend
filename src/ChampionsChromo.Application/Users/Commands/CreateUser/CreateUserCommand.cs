using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
