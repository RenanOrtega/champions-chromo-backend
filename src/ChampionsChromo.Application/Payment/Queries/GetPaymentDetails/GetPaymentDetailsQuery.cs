using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Payment.Queries.GetPaymentDetails;

public record GetPaymentDetailsQuery(string PaymentIntentId) : IRequest<Result<PaymentDetailsResponse>>;
