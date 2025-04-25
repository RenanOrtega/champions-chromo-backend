using ChampionsChromo.Application.Pix.Commands.CreateOrder;
using ChampionsChromo.Application.Pix.Commands.UpdateOrderStatus;
using ChampionsChromo.Application.Pix.Queries.GetPixOrderStatus;
using ChampionsChromo.Core.Models;
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
    public async Task<IActionResult> StatusPix([FromBody] WebhookAbacatePayPixRequest request, [FromQuery] string webhookSecret)
    {
        if (string.IsNullOrEmpty(webhookSecret) || webhookSecret != _webhookSecret)
            return Unauthorized();

        var result = await _mediator.Send(new UpdateOrderStatusCommand(request.PixQrCode.Id, request.PixQrCode.Status));

        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.Error);
    }

    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.Error);
    }

    [HttpGet("order/status")]
    public async Task<IActionResult> CreateOrder([FromQuery] string integrationId)
    {
        var result = await _mediator.Send(new GetPixOrderStatusQuery(integrationId));

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }
}   
