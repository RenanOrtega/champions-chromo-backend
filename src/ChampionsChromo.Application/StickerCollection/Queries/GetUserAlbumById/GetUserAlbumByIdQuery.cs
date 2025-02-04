using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbumById;

public record GetUserAlbumByIdQuery(string Id) : IRequest<Result<UserAlbumDto>>;
