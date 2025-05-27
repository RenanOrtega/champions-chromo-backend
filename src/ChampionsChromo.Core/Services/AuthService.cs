using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChampionsChromo.Core.Services;

public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Usuário não encontrado"
                };
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Senha incorreta"
                };
            }

            // Atualiza último login
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);

            return new AuthResult
            {
                Success = true,
                Message = "Login realizado com sucesso",
                Token = token,
                User = user
            };
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                Message = $"Erro interno: {ex.Message}"
            };
        }
    }

    public async Task<AuthResult> RegisterAsync(string username, string password)
    {
        try
        {
            if (await _userRepository.ExistsAsync(username))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Usuário já existe"
                };
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

            var token = GenerateJwtToken(user);

            return new AuthResult
            {
                Success = true,
                Message = "Usuário criado com sucesso",
                Token = token,
                User = user
            };
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                Message = $"Erro interno: {ex.Message}"
            };
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<User?> GetCurrentUserAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);

            var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "id");
            if (userIdClaim == null) return null;

            return await _userRepository.GetByIdAsync(userIdClaim.Value);
        }
        catch
        {
            return null;
        }
    }

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new("id", user.Id),
            new("username", user.Username),
        };

        // Adiciona roles como claims
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Token válido por 7 dias
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
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

