using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Dashboard.Queries.GetMetrics;

public class GetMetricsQuery : IRequest<Result<MetricsDto>>
{
    /// <summary>Início do período (inclusivo, UTC). Nulo = sem limite inicial.</summary>
    public DateTime? From { get; set; }

    /// <summary>Fim do período (exclusivo, UTC). Nulo = sem limite final.</summary>
    public DateTime? To { get; set; }
}
