using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
            return Unauthorized();
        }

        SetAuthCookie(result.Value.Token);

        return Ok(new
        {
            message = result.Value.Message,
            user = new
            {
                id = result.Value.User!.Id,
                username = result.Value.User.Username,
                roles = result.Value.User.Roles
            }
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Value.Message });
        }

        SetAuthCookie(result.Value.Token);

        return Ok(new
        {
            message = result.Value.Message,
            user = new
            {
                id = result.Value.User!.Id,
                username = result.Value.User.Username,
                roles = result.Value.User.Roles
            }
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok(new { message = "Logout realizado com sucesso" });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var token = GetTokenFromCookie();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Token não encontrado" });
        }

        var query = new GetCurrentUserQuery(token);
        var result = await _mediator.Send(query);

        if (result.Value == null)
        {
            return Unauthorized(new { message = "Usuário não autenticado" });
        }

        return Ok(new
        {
            id = result.Value.Id,
            username = result.Value.Username,
            roles = result.Value.Roles,
            lastLoginAt = result.Value.LastLoginAt
        });
    }

    private void SetAuthCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        Response.Cookies.Append("AuthToken", token, cookieOptions);
    }

    private string? GetTokenFromCookie()
    {
        return Request.Cookies["AuthToken"];
    }
}
