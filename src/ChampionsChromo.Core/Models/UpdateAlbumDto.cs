namespace ChampionsChromo.Core.Models
{
    public class UpdateAlbumDto
    {
        public string Name { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public bool HasCommon { get; set; }
        public bool HasLegend { get; set; }
        public bool HasA4 { get; set; }
        public int CommonPrice { get; set; }
        public int LegendPrice { get; set; }
        public int A4Price { get; set; }
        public int TotalStickers { get; set; }
    }
}
