using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Models;

public class OrderSummaryDto
{
    public string Id { get; set; } = string.Empty;
    public IList<SchoolOrderDto> Schools { get; set; } = [];
    public CustomerDto? Customer { get; set; }
    public int TotalAlbums { get; set; }
    public int TotalStickers { get; set; }
    public int PriceTotal { get; set; }
}

public class SchoolOrderDto
{
    public string SchoolId { get; set; } = string.Empty;
    public IList<AlbumOrderDto> Albums { get; set; } = [];
}

public class AlbumOrderDto
{
    public string AlbumId { get; set; } = string.Empty;
    public IList<StickersOrderDto> Stickers { get; set; } = [];
}

public class StickersOrderDto
{
    public StickerType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
