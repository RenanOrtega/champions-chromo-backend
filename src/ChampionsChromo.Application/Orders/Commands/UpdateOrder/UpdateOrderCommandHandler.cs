using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<UpdateOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var updateOrderDto = new UpdateOrderDto
        {

        };

        try
        {
            await _orderRepository.UpdateAsync(request.Id, updateOrderDto);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create order: {ex.Message}");
        }
    }
}
