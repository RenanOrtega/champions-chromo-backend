using ChampionsChromo.Api.Payments;
using ChampionsChromo.Application.Payments.Commands.CreatePixPayment;
using ChampionsChromo.Application.Payments.Commands.ProcessMercadoPagoNotification;
using ChampionsChromo.Application.Payments.Queries.GetPaymentStatus;
using ChampionsChromo.Core.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(
    IMediator mediator,
    IOptions<MercadoPagoOptions> options,
    ILogger<PaymentController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly MercadoPagoOptions _options = options.Value;
    private readonly ILogger<PaymentController> _logger = logger;

    // Generates (or returns the still valid) Pix for an order. The amount always comes from the order in the database.
    [HttpPost("{orderId}/pix")]
    [EnableRateLimiting("payment")]
    public async Task<IActionResult> CreatePix([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreatePixPaymentCommand(orderId), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    // Polled by the store while the customer is on the Pix screen.
    [HttpGet("{orderId}")]
    [EnableRateLimiting("payment-status")]
    public async Task<IActionResult> GetStatus([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPaymentStatusQuery(orderId), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    // Webhook configured in the Mercado Pago application (topic "Order (Mercado Pago)").
    // URL: https://api.albumdefigurinha.com.br/api/payment/webhook/mercadopago
    [HttpPost("webhook/mercadopago")]
    public async Task<IActionResult> MercadoPagoWebhook(CancellationToken cancellationToken)
    {
        var dataId = Request.Query["data.id"].ToString();
        var type = Request.Query["type"].ToString();
        var signature = Request.Headers["x-signature"].ToString();
        var requestId = Request.Headers["x-request-id"].ToString();

        if (!MercadoPagoSignature.IsValid(signature, requestId, dataId, _options.WebhookSecret))
        {
            _logger.LogWarning("Rejected Mercado Pago webhook with invalid signature (type {Type}, id {DataId})", type, dataId);
            return Unauthorized();
        }

        if (!string.Equals(type, "order", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(dataId))
            return Ok();

        try
        {
            await _mediator.Send(new ProcessMercadoPagoNotificationCommand(dataId), cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            // Non-2xx makes Mercado Pago retry the notification later.
            _logger.LogError(ex, "Failed to process Mercado Pago notification {DataId}", dataId);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}
