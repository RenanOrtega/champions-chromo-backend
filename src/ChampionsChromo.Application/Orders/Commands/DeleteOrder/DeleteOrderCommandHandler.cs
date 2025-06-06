using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<DeleteOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _orderRepository.DeleteAsync(request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to delete order: {ex.Message}");
        }
    }
}
