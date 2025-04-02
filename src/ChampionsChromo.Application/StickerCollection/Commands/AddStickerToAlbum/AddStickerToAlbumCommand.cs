using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.AddStickerToAlbum;

public record AddStickerToAlbumCommand(
    string UserId,
    string AlbumId,
    int StickerNumber,
    string StickerType) : IRequest<Result>; 