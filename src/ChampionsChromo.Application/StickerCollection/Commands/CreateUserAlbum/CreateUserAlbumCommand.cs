using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.CreateUserAlbum;

public record CreateUserAlbumCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
    public List<UserAlbumEntry> Albums { get; set; } = [];
}
