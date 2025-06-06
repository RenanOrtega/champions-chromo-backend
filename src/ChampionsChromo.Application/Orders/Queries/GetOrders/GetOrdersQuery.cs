using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery : IRequest<Result<IEnumerable<OrderSummaryDto>>>;
