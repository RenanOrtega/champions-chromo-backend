using System.Text.Json.Serialization;

namespace ChampionsChromo.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CupomType
{
    Percent = 0,
    Fixed = 1,
    FreeShipping = 2
}
