using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Application.Extensions;

public static class OrderSummaryExtensions
{

    public static OrderSummaryDto ToDto(this OrderSummary order)
    {
        var schools = order.Albums
            .GroupBy(a => a.SchoolId)
            .Select(static schoolGroup => new SchoolOrderDto
            {
                SchoolId = schoolGroup.Key,
                Albums = [.. schoolGroup.Select(album => new AlbumOrderDto
                {
                    AlbumId = album.AlbumId,
                    Stickers = [.. album.Stickers.Select(sticker => new StickersOrderDto
                    {
                        Type = sticker.Type,
                        Number = sticker.Number,
                        Quantity = sticker.Quantity
                    })]
                })],
            }).ToList();

        var customer = new CustomerDto
        {
            Email = order.Customer?.Email ?? string.Empty,
            Name = order.Customer?.Name ?? string.Empty,
            Address = order.Customer?.Address is not null ? new CustomerAddressDto
            {
                Street = order.Customer.Address.Street,
                Number = order.Customer.Address.Number,
                Neighborhood = order.Customer.Address.Neighborhood,
                PostalCode = order.Customer.Address.PostalCode,
                State = order.Customer.Address.State,
                Complement = order.Customer.Address.Complement,
                City = order.Customer.Address.City
            } : null

        };

        return new OrderSummaryDto
        {
            Id = order.Id,
            Schools = schools,
            Customer = customer,
            TotalAlbums = order.Albums.Count,
            TotalStickers = order.Albums.SelectMany(a => a.Stickers).Sum(s => s.Quantity),
            PriceTotal = order.PriceTotal
        };
    }
}
