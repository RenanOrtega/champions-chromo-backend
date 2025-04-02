using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;

public record GetStickerCollecionByUserIdQuery(string UserId) : IRequest<Result<UserAlbumDto>>;
