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

        return new OrderSummaryDto
        {
            Id = order.Id,
            Schools = schools,
            TotalAlbums = order.Albums.Count,
            TotalStickers = order.Albums.SelectMany(a => a.Stickers).Sum(s => s.Quantity),
            PriceTotal = order.PriceTotal
        };
    }
}
