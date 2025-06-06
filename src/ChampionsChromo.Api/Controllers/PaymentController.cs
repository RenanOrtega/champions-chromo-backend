using ChampionsChromo.Application.Payment.Commands.CreatePaymentIntent;
using ChampionsChromo.Application.Payment.Commands.WebhookPayment;
using ChampionsChromo.Application.Payment.Queries.GetPaymentDetails;
using ChampionsChromo.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IMediator mediator, ILogger<PaymentController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<PaymentController> _logger = logger;

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreateOrder([FromBody] CreatePaymentIntentCommand request)
    {
        try
        {
            if (request.Amount < 50)
            {
                return BadRequest(new { error = "Valor mínimo não atendido" });
            }

            var result = await _mediator.Send(request);
            return Ok(result.Value);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Erro de aplicação ao criar Payment Intent");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao criar Payment Intent");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpGet("{paymentIntentId}")]
    public async Task<ActionResult<PaymentDetailsResponse>> GetPaymentDetails([FromRoute] string paymentIntentId)
    {
        try
        {

            var result = await _mediator.Send(new GetPaymentDetailsQuery(paymentIntentId));

            if (result.Value == null)
            {
                return NotFound(new { error = "Pagamento não encontrado" });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar detalhes do pagamento {PaymentIntentId}", paymentIntentId);
            return StatusCode(500, new { error = "Erro ao buscar pagamento" });
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook()
    {
        try
        {
            var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(signature))
            {
                return BadRequest("Missing Stripe-Signature header");
            }

            string payload;
            using (var reader = new StreamReader(Request.Body))
            {
                payload = await reader.ReadToEndAsync();
            }

            var result = await _mediator.Send(new WebhookPaymentCommand(payload, signature));

            return Ok(new { received = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar webhook do Stripe");
            return StatusCode(500, "Internal server error");
        }
    }

}
