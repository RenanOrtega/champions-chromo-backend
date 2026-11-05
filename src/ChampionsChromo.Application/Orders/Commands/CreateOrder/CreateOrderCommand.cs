using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.CreateOrder;

// Prices, album names and schools are read from the database, never from the request.
public class CreateOrderCommand : IRequest<Result<CreateOrderSummaryDto>>
{
    public IList<CreateAlbumOrderCommand> Albums { get; set; } = [];
    public string? CouponCode { get; set; }
    public CustomerCommand? Customer { get; set; }
}

public class CreateAlbumOrderCommand
{
    public string AlbumId { get; set; } = string.Empty;
    public IList<CreateStickerOrderItemCommand> Stickers { get; set; } = [];
}

public class CreateStickerOrderItemCommand
{
    public StickerType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class CustomerCommand
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public CustomerAddressCommand? Address { get; set; }
}

public class CustomerAddressCommand
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;

}
