using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbums;

public record GetUserAlbumsQuery : IRequest<Result<IEnumerable<UserAlbumDto>>>;
