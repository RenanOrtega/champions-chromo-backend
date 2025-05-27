using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Auth.Queries;
using MediatR;
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
            return Unauthorized(result.Error);
        }

        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var refreshToken = Request.Cookies["REFRESH_TOKEN"];
            var result = await _mediator.Send(new RefreshCommand(refreshToken));

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.Error);
                return Unauthorized(result.Error);
            }

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        await _mediator.Send(command);

        return Created();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(-1) 
        };

        Response.Cookies.Append("ACCESS_TOKEN", "", cookieOptions);
        Response.Cookies.Append("REFRESH_TOKEN", "", cookieOptions);
        
        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var token = GetTokenFromCookie();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
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

    private string? GetTokenFromCookie()
    {
        return Request.Cookies["ACCESS_TOKEN"];
    }
}
