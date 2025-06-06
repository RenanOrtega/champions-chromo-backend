using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Application.Extensions;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrderByIdQuery, Result<OrderSummaryDto>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<OrderSummaryDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);

        if (order == null)
            return Result<OrderSummaryDto>.Failure($"Order with ID {request.Id} not found.");

        return Result<OrderSummaryDto>.Success(order.ToDto());
    }
}
