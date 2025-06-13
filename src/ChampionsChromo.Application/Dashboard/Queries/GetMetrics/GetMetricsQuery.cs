using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Dashboard.Queries.GetMetrics;

public class GetMetricsQuery : IRequest<Result<MetricsDto>>
{
    public int DaysBack { get; set; } = 30;
}
