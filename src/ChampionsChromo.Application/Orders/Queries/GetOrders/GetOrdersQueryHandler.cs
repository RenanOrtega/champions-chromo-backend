using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Application.Extensions;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersQuery, Result<IEnumerable<OrderSummaryDto>>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<IEnumerable<OrderSummaryDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync();

        return Result<IEnumerable<OrderSummaryDto>>.Success(orders.Select(x => x.ToDto()));
    }
}
