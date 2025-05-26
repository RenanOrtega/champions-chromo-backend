using AutoMapper;
using ChampionsChromo.Application.Albums.Queries;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Dashboard.Queries.GetMetrics;

public class GetMetricsQueryHandler(IAlbumRepository albumRepository, ISchoolRepository schoolRepository, IMapper mapper) : IRequestHandler<GetMetricsQuery, Result<MetricsDto>>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;
    private readonly ISchoolRepository _schoolRepository = schoolRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<MetricsDto>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        var albumsCount = await _albumRepository.CountAsync();
        var schoolsCount = await _schoolRepository.CountAsync();

        return Result<MetricsDto>.Success(new MetricsDto { AlbumsCount = albumsCount, SchoolsCount = schoolsCount });
    }
}
