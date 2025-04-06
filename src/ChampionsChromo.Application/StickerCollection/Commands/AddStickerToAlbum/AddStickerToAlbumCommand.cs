using ChampionsChromo.Application.Commands.Interfaces;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.AddStickerToAlbum;

public class AddStickerToAlbumCommand(string albumId, int stickerNumber, StickerType stickerType) : IUserCommand, IRequest<Result>
{
    public string AlbumId { get; set; } = albumId;
    public int StickerNumber { get; set; } = stickerNumber;
    public StickerType StickerType { get; set; } = stickerType;
    public string UserId { get; set; } = string.Empty;
} 