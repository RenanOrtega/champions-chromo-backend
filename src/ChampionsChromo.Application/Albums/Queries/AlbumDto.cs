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
    public List<CommonSticker> CommonStickers { get; set; } = [];
    public List<FrameSticker> FrameStickers { get; set; } = [];
    public List<LegendSticker> LegendStickers { get; set; } = [];
    public List<A4Sticker> A4Stickers { get; set; } = [];
    public int TotalStickers { get; set; }
}
