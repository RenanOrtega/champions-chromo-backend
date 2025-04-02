using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.RemoveStickerFromAlbum;

public record RemoveStickerFromAlbumCommand(
    string UserId,
    string AlbumId,
    int StickerNumber,
    string StickerType) : IRequest<Result>; 