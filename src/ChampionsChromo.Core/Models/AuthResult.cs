using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public User? User { get; set; }
}
