using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.AddStickerToAlbum;

public class AddStickerToAlbumCommandHandler(IUserAlbumRepository userAlbumRepository) 
    : IRequestHandler<AddStickerToAlbumCommand, Result>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;

    public async Task<Result> Handle(AddStickerToAlbumCommand request, CancellationToken cancellationToken)
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

        if (stickerList.Contains(request.StickerNumber))
            return Result.Failure("Sticker already exists in collection");

        stickerList.Add(request.StickerNumber);
        await _userAlbumRepository.UpdateAsync(userAlbum.Id, userAlbum);

        return Result.Success();
    }

    private static List<int>? GetStickerList(UserAlbumEntry entry, string stickerType) => stickerType.ToLower() switch
    {
        "comum" => entry.OwnedCommonStickers,
        "quadro" => entry.OwnedFrameStickers,
        "legends" => entry.OwnedLegendStickers,
        "a4" => entry.OwnedA4Stickers,
        _ => null
    };
} 