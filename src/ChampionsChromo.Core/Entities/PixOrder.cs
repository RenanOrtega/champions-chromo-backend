using ChampionsChromo.Core.Entities.Pix;
using ChampionsChromo.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ChampionsChromo.Core.Entities;

public class PixOrder : Entity
{
    public string IntegrationId { get; set; }
    public Payment Payment { get; set; }
    public Address Address { get; set; }
    public Customer Customer { get; set; }
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public PixStatus Status { get; set; }
}
