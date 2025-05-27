using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(string username, string password);
    Task LoginAsync(string username, string password);
    Task RefreshTokenAsync(string? refreshToken);
    (string, DateTime) GenerateJwtToken(User user);
    string GenerateRefreshToken();
    void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
    Task<User?> GetCurrentUserAsync(string token);
    bool VerifyPassword(string password, string hash);
    string HashPassword(string password);
}
