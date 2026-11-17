using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.ArchiveOrders;

public class ArchiveOrdersCommandHandler(IOrderRepository orderRepository) : IRequestHandler<ArchiveOrdersCommand, Result<long>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<long>> Handle(ArchiveOrdersCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var updated = await _orderRepository.SetArchivedAsync(ids, request.Archived ? DateTime.UtcNow : null);

        return Result<long>.Success(updated);
    }
}
