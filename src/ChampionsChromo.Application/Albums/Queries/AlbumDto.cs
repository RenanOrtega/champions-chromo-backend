using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Application.Albums.Queries;

public class AlbumDto
{
    public string Id { get; set; } = string.Empty;
    public string SchoolId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverImage { get; set; } = string.Empty;
    public int TotalStickers { get; set; }
}
