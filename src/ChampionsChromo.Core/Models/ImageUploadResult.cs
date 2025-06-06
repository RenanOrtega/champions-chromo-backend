namespace ChampionsChromo.Core.Models;

public class ImageUploadResult
{
    public bool Success { get; set; }
    public string? Key { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? ContentType { get; set; }
}
