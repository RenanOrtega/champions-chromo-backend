using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(string Id) : IRequest<Result<OrderSummaryDto>>;
