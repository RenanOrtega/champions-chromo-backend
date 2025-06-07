using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<Result<CreateOrderSummaryDto>>
{
    public IList<CreateAlbumOrderCommand> Albums { get; set; }
    public int PriceTotal { get; set; }
}

public class CreateAlbumOrderCommand
{
    public string AlbumId { get; set; }
    public string SchoolId { get; set; }
    public IList<CreateStickerOrderItemCommand> Stickers { get; set; }
}

public class CreateStickerOrderItemCommand
{
    public StickerType Type { get; set; }
    public string Number { get; set; }
    public int Quantity { get; set; }
}
