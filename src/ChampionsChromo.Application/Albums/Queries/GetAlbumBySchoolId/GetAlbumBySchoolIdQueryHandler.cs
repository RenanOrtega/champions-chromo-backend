using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Albums.Queries.GetAlbumBySchoolId;

public class GetAlbumBySchoolIdQueryHandler(IAlbumRepository albumRepository, IMapper mapper) : IRequestHandler<GetAlbumBySchoolIdQuery, Result<IEnumerable<AlbumDto>>>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<AlbumDto>>> Handle(GetAlbumBySchoolIdQuery request, CancellationToken cancellationToken)
    {
        var album = await _albumRepository.FindBySchoolIdAsync(request.schoolId);

        if (album == null)
            return Result<IEnumerable<AlbumDto>>.Failure($"Album with SchoolId {request.schoolId} not found.");

        return Result<IEnumerable<AlbumDto>>.Success(_mapper.Map<IEnumerable<AlbumDto>>(album));
    }
}
