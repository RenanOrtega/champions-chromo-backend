using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Albums.Queries.GetAlbumById;

public class GetAlbumByIdQueryHandler(IAlbumRepository albumRepository, IMapper mapper) : IRequestHandler<GetAlbumByIdQuery, Result<AlbumDto>>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<AlbumDto>> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
    {
        var album = await _albumRepository.GetByIdAsync(request.Id);

        if (album == null)
            return Result<AlbumDto>.Failure($"Album with ID {request.Id} not found.");

        return Result<AlbumDto>.Success(_mapper.Map<AlbumDto>(album));
    }
}
