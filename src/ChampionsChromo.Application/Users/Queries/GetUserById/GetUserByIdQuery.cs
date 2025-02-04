using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<Result<UserDto>>;
