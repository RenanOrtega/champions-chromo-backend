using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Albums.Queries.GetAlbums;

public class GetAlbumsQueryHandler(IAlbumRepository albumRepository, IMapper mapper) : IRequestHandler<GetAlbumsQuery, Result<IEnumerable<AlbumDto>>>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<AlbumDto>>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
    {
        var albums = await _albumRepository.GetAllAsync();

        return Result<IEnumerable<AlbumDto>>.Success(_mapper.Map<IEnumerable<AlbumDto>>(albums));
    }
}
