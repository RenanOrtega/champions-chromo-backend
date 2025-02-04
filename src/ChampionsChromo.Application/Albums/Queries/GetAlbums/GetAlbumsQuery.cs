using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Albums.Queries.GetAlbums;

public record GetAlbumsQuery : IRequest<Result<IEnumerable<AlbumDto>>>;
