using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Payments.Queries.GetPaymentStatus;

public record GetPaymentStatusQuery(string OrderId) : IRequest<Result<PixPaymentDto>>;
