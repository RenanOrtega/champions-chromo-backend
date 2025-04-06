using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.RemoveStickerFromAlbum;

public class RemoveStickerFromAlbumCommandHandler(IUserAlbumRepository userAlbumRepository) 
    : IRequestHandler<RemoveStickerFromAlbumCommand, Result>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;

    public async Task<Result> Handle(RemoveStickerFromAlbumCommand request, CancellationToken cancellationToken)
    {
        var userAlbum = await _userAlbumRepository.GetByUserIdAsync(request.UserId);
        
        if (userAlbum == null)
            return Result.Failure("User album not found");

        var albumEntry = userAlbum.Albums.FirstOrDefault(a => a.AlbumId == request.AlbumId);
        if (albumEntry == null)
            return Result.Failure("Album not found in user collection");

        var stickerList = GetStickerList(albumEntry, request.StickerType);
        if (stickerList == null)
            return Result.Failure("Invalid sticker type");

        if (!stickerList.Contains(request.StickerNumber))
            return Result.Failure("Sticker not found in collection");

        stickerList.Remove(request.StickerNumber);
        await _userAlbumRepository.UpdateAsync(userAlbum.Id, userAlbum);

        return Result.Success();
    }

    private static List<int>? GetStickerList(UserAlbumEntry entry, StickerType stickerType) => stickerType switch
    {
        StickerType.Comum => entry.OwnedCommonStickers,
        StickerType.Quadro => entry.OwnedFrameStickers,
        StickerType.Legends => entry.OwnedLegendStickers,
        StickerType.A4 => entry.OwnedA4Stickers,
        _ => null
    };
} 