using ChampionsChromo.Application.Albums.Queries;
using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Dashboard.Queries.GetMetrics;

public record GetMetricsQuery : IRequest<Result<MetricsDto>>;
