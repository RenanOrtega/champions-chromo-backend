using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Albums.Commands.DeleteAlbum
{
    public record DeleteAlbumCommand(string Id) : IRequest<Result>;
}
