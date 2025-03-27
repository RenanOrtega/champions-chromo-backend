namespace ChampionsChromo.Core.Entities;

public class UserAlbum : Entity
{
    public string UserId { get; set; } = string.Empty;
    public List<UserAlbumEntry> Albums { get; set; } = [];
}

public class UserAlbumEntry
{
    public string AlbumId { get; set; } = string.Empty;
    public List<int> OwnedCommonStickers { get; set; } = [];
    public List<int> OwnedFrameStickers { get; set; } = [];
    public List<int> OwnedLegendStickers { get; set; } = [];
    public List<int> OwnedA4Stickers { get; set; } = [];
}
