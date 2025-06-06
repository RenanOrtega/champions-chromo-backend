using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Payment.Commands.CreatePaymentIntent;

public class CreatePaymentIntentCommandHandler(IStripeService stripeService) : IRequestHandler<CreatePaymentIntentCommand, Result<CreatePaymentIntentResponse>>
{
    private readonly IStripeService _stripeService = stripeService;

    public async Task<Result<CreatePaymentIntentResponse>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        var createPaymentIntentRequest = new CreatePaymentIntentRequest
        {
            Amount = request.Amount,
            Currency = request.Currency,
            Items = request.Items
        };
        
        var createPaymentIntentResponse = await _stripeService.CreatePaymentIntentAsync(createPaymentIntentRequest);

        return Result<CreatePaymentIntentResponse>.Success(createPaymentIntentResponse);
    }
}
