namespace ChampionsChromo.Application.StickerCollection.Queries;

public class UserAlbumDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public List<UserAlbumEntryDto> Albums { get; set; } = [];
}

public class UserAlbumEntryDto
{
    public string AlbumId { get; set; } = string.Empty;
    public List<int> OwnedCommonStickers { get; set; } = [];
    public List<int> OwnedFrameStickers { get; set; } = [];
    public List<int> OwnedLegendStickers { get; set; } = [];
    public List<int> OwnedA4Stickers { get; set; } = [];
}
