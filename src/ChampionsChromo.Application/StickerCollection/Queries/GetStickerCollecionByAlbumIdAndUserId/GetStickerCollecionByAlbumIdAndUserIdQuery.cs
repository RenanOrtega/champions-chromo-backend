using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;

public record GetStickerCollecionByAlbumIdAndUserIdQuery(string AlbumId, string UserId) : IRequest<Result<IEnumerable<UserAlbumDto>>>;
