using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Options;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChampionsChromo.Core.Services;

public class AuthService(
    IUserRepository userRepository,
    IOptions<JwtOptions> jwtOptions,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user is null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Falha no login.");
        }

        var (jwtToken, expirationDateInUtc) = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.LastLoginAt = DateTime.UtcNow;
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

        await _userRepository.UpdateAsync(user);

        WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", refreshToken, refreshTokenExpirationDateInUtc);
    }

    public async Task RefreshTokenAsync(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _userRepository.GetByRefreshToken(refreshToken);
        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }

        if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException();

        }

        var (jwtToken, expirationDateInUtc) = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.LastLoginAt = DateTime.UtcNow;
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

        await _userRepository.UpdateAsync(user);

        WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", newRefreshToken, refreshTokenExpirationDateInUtc);
    }

    public async Task RegisterAsync(string username, string password)
    {
        if (await _userRepository.ExistsAsync(username))
        {
            throw new Exception("Usuário já existe.");
        }

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Roles = ["user"],
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.CreateAsync(user);

        var (jwtToken, expirationDateInUtc) = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.LastLoginAt = DateTime.UtcNow;
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

        await _userRepository.UpdateAsync(user);

        WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", refreshToken, refreshTokenExpirationDateInUtc);
    }

    public async Task<User?> GetCurrentUserAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);

            var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null) return null;

            return await _userRepository.GetByIdAsync(userIdClaim.Value);
        }
        catch
        {
            return null;
        }
    }

    public (string, DateTime) GenerateJwtToken(User user)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Username)
        };

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: expires,
            signingCredentials: credentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwtToken, expires);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Expires = expiration,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

