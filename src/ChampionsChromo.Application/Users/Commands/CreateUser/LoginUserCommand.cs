using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Users.Commands.CreateUser;

public record LoginUserCommand : IRequest<Result<string>>
{
    public string FirebaseId { get; set; } = string.Empty;
}
