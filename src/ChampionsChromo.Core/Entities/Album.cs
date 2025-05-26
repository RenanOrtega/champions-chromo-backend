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
    public int TotalStickers { get; set; }
    public bool HasCommon { get; set; }
    public bool HasLegend { get; set; }
    public bool HasA4 { get; set; }
}
