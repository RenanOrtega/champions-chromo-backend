using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;
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
        {
            return Result.Failure($"User album not found for user {request.UserId}");
        }

        var albumEntry = userAlbum.Albums.FirstOrDefault(a => a.AlbumId == request.AlbumId);
        if (albumEntry == null)
        {
            try
            {
                userAlbum = await _userAlbumRepository.AddAlbumToUser(request.UserId, request.AlbumId);
                albumEntry = userAlbum.Albums.FirstOrDefault(a => a.AlbumId == request.AlbumId);

                if (albumEntry == null)
                {
                    return Result.Failure("Failed to add album to user");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred while adding album: {ex.Message}");
            }
        }

        var stickerList = GetStickerList(albumEntry, request.StickerType);
        if (stickerList == null)
        {
            return Result.Failure($"Invalid sticker type: {request.StickerType}");
        }

        if (stickerList.Contains(request.StickerNumber))
        {
            return Result.Failure($"Sticker {request.StickerNumber} already exists in the collection");
        }

        stickerList.Add(request.StickerNumber);

        try
        {
            await _userAlbumRepository.UpdateAsync(userAlbum.Id, userAlbum);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while updating album: {ex.Message}");
        }
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