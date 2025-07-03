namespace ChampionsChromo.Core.Models;

public class AlbumDto
{
    public string Id { get; set; } = string.Empty;
    public string SchoolId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverImage { get; set; } = string.Empty;
    public int TotalStickers { get; set; }
    public bool HasCommon { get; set; }
    public bool HasLegend { get; set; }
    public bool HasA4 { get; set; }
    public decimal CommonPrice { get; set; }
    public decimal LegendPrice { get; set; }
    public decimal A4Price { get; set; }

}
