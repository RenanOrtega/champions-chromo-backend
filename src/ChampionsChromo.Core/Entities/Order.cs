using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Entities;

public class OrderSummary : Entity
{
    public IList<AlbumOrder> Albums { get; set; }
    public int PriceTotal { get; set; }
}

public class AlbumOrder
{
    public string AlbumId { get; set; }
    public string SchoolId { get; set; }
    public IList<StickerOrderItem> Stickers { get; set; }
}

public class StickerOrderItem
{
    public StickerType Type { get; set; }
    public string Number { get; set; }
    public int Quantity { get; set; }
}
