using System.Text.Json.Serialization;
using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Albums.Commands.UpdateAlbum;

public record UpdateAlbumCommmand : IRequest<Result>
{
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public bool HasCommon { get; set; }
    public bool HasLegend { get; set; }
    public bool HasA4 { get; set; }
    public int TotalStickers { get; set; }
}
