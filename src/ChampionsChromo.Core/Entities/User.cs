namespace ChampionsChromo.Core.Entities;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAtUtc { get; set; }
    public List<string> Roles { get; set; } = [];
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
}
