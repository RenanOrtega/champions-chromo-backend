using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using FirebaseAdmin.Auth;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChampionsChromo.Application.Users.Commands.CreateUser;

public class LoginUserCommandHandler(IUserRepository userRepository, IConfiguration configuration) : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var token = await AuthenticateGoogleUserAsync(request.FirebaseId);
        return Result<string>.Success(token);
    }

    private async Task<string> AuthenticateGoogleUserAsync(string googleIdToken)
    {
        var user = await GetOrCreateUserFromGoogleTokenAsync(googleIdToken);
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("GoogleId", user.FirebaseId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(7);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<User> GetOrCreateUserFromGoogleTokenAsync(string googleIdToken)
    {
        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(googleIdToken);
            string uid = decodedToken.Uid;

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            var user = await _userRepository.GetByFirebaseIdAsync(uid);
            if (user == null)
            {
                user = new User
                {
                    FirebaseId = uid,
                    Email = userRecord.Email,
                    Name = userRecord.DisplayName,
                    PhotoUrl = userRecord.PhotoUrl
                };

                await _userRepository.AddAsync(user);
                return user;
            }

            user.Email = userRecord.Email;
            user.Name = userRecord.DisplayName;
            user.PhotoUrl = userRecord.PhotoUrl;

            await _userRepository.UpdateAsync(user.Id, user);
            return user;
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException($"Bad login :C - {ex.Message}");
        }
    }
}
