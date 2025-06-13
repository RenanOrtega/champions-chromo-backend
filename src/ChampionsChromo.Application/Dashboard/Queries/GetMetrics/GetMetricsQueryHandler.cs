using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Dashboard.Queries.GetMetrics;

public class GetMetricsQueryHandler(IAlbumRepository albumRepository, ISchoolRepository schoolRepository, IMapper mapper, IOrderRepository orderRepository) : IRequestHandler<GetMetricsQuery, Result<MetricsDto>>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;
    private readonly ISchoolRepository _schoolRepository = schoolRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<MetricsDto>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var metrics = await _orderRepository.GetDashboardMetricsAsync(request.DaysBack);

            // Manter as métricas existentes
            metrics.AlbumsCount = await _albumRepository.CountAsync();
            metrics.SchoolsCount = await _schoolRepository.CountAsync();

            return Result<MetricsDto>.Success(metrics);
        }
        catch (Exception ex)
        {
            return Result<MetricsDto>.Failure($"Erro ao obter métricas: {ex.Message}");
        }
    }
}
