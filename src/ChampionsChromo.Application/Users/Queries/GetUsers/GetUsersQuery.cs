using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;
