using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.ArchiveOrders;

// Archive = soft delete. Archived = false brings the orders back to the list.
public class ArchiveOrdersCommand : IRequest<Result<long>>
{
    public IList<string> Ids { get; set; } = [];
    public bool Archived { get; set; } = true;
}
