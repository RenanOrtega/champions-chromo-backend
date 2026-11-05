using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Payments.Commands.CreatePixPayment;

public record CreatePixPaymentCommand(string OrderId) : IRequest<Result<PixPaymentDto>>;
