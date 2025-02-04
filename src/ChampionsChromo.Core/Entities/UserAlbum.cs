namespace ChampionsChromo.Core.Entities;

public class UserAlbum : Entity
{
    public string UserId { get; set; } = string.Empty;
    public List<UserAlbumEntry> Albums { get; set; } = [];
}

public class UserAlbumEntry
{
    public string AlbumId { get; set; } = string.Empty;
    public List<int> OwnedStickers { get; set; } = [];
}
