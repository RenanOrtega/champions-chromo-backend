using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Entities;

public class OrderSummary : Entity
{
    public IList<AlbumOrder> Albums { get; set; } = [];
    public Customer? Customer { get; set; }
    public int PriceTotal { get; set; }
}

public class AlbumOrder
{
    public string AlbumId { get; set; } = string.Empty;
    public string SchoolId { get; set; } = string.Empty;
    public IList<StickerOrderItem> Stickers { get; set; } = [];
}

public class StickerOrderItem
{
    public StickerType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
