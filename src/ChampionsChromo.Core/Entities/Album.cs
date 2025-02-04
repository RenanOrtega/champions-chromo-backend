using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChampionsChromo.Core.Entities;

public class Album : Entity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string SchoolId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverImage { get; set; } = string.Empty;
    public List<CommonSticker> CommonStickers { get; set; } = [];
    public List<FrameSticker> FrameStickers { get; set; } = [];
    public List<LegendSticker> LegendStickers { get; set; } = [];
    public List<A4Sticker> A4Stickers { get; set; } = [];
    public int TotalStickers =>
         CommonStickers.Count + FrameStickers.Count + LegendStickers.Count + A4Stickers.Count;

}
