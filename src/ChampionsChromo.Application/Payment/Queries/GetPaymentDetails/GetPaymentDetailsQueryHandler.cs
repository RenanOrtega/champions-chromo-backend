using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Services.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Payment.Queries.GetPaymentDetails;

public class GetPaymentDetailsQueryHandler(IStripeService stripeService) : IRequestHandler<GetPaymentDetailsQuery, Result<PaymentDetailsResponse>>
{
    private readonly IStripeService _stripeService = stripeService;

    public async Task<Result<PaymentDetailsResponse>> Handle(GetPaymentDetailsQuery request, CancellationToken cancellationToken)
    {
        var details = await _stripeService.GetPaymentDetailsAsync(request.PaymentIntentId);

        if (details == null)
        {
            return Result<PaymentDetailsResponse>.Failure("Pagamento não encontrado");
        }

        return Result<PaymentDetailsResponse>.Success(details);
    }
}
