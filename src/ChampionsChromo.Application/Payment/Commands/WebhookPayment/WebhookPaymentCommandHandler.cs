using System.Text.Json;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Payment.Commands.WebhookPayment;

public class WebhookPaymentCommandHandler(IStripeService stripeService) : IRequestHandler<WebhookPaymentCommand, Result>
{
    private readonly IStripeService _stripeService = stripeService;

    public async Task<Result> Handle(WebhookPaymentCommand request, CancellationToken cancellationToken)
    {
        var isValid = await _stripeService.ValidateWebhookAsync(request.Payload, request.Signature);
        if (!isValid)
        {
            return Result.Failure("Invalid signature");
        }

        var stripeEvent = JsonSerializer.Deserialize<WebhookEvent>(request.Payload);
        if (stripeEvent != null)
        {
            await _stripeService.ProcessWebhookEventAsync(stripeEvent.Type, stripeEvent.Data);
        }

        return Result.Success();
    }
}
