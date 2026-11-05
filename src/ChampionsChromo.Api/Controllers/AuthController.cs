using ChampionsChromo.Application.Auth.Commands;
using ChampionsChromo.Application.Auth.Queries;
using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IWebHostEnvironment environment) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IWebHostEnvironment _environment = environment;

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return Unauthorized(result.Error);

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var refreshToken = Request.Cookies["REFRESH_TOKEN"];
            var result = await _mediator.Send(new RefreshCommand(refreshToken));

            if (!result.IsSuccess)
                return Unauthorized(result.Error);

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    // Account creation is only available when running locally (Development).
    // In production admins are created directly in the database.
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        await _mediator.Send(command);

        return Created();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand(Request.Cookies["REFRESH_TOKEN"]));

        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetCurrentUserQuery(userId));

        if (result.Value == null)
            return Unauthorized(new { message = "Usuário não autenticado" });

        return Ok(new
        {
            id = result.Value.Id,
            username = result.Value.Username,
            roles = result.Value.Roles,
            lastLoginAt = result.Value.LastLoginAt
        });
    }
}
