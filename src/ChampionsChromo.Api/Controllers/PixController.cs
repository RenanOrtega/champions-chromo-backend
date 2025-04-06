using ChampionsChromo.Application.Pix.Commands.CreateStatusPix;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PixController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly string _webhookSecret = configuration["WebhookSettings:Secret"];

    [HttpPost("webhook")]
    public async Task<IActionResult> StatusPix([FromBody] CreateStatusPixCommand command, [FromQuery] string webhookSecret)
    {
        if (string.IsNullOrEmpty(webhookSecret) || webhookSecret != _webhookSecret)
            return Unauthorized();

        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.Error);
    }
}
