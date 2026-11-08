using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.Payments.Queries.GetPaymentConfig;

public record GetPaymentConfigQuery : IRequest<PaymentConfigDto>;

public record PaymentConfigDto(PaymentMode Mode);

public class GetPaymentConfigQueryHandler(IPaymentModeProvider paymentModeProvider) : IRequestHandler<GetPaymentConfigQuery, PaymentConfigDto>
{
    public Task<PaymentConfigDto> Handle(GetPaymentConfigQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(new PaymentConfigDto(paymentModeProvider.Current));
}
