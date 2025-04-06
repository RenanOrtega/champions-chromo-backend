using ChampionsChromo.Application.Commands.Interfaces;
using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;

public class GetStickerCollecionByUserIdQuery() : IUserCommand, IRequest<Result<UserAlbumDto>>
{
    public string UserId { get; set; } = string.Empty;
}

