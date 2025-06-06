using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Payment.Commands.WebhookPayment;

public record WebhookPaymentCommand(string Payload, string Signature) : IRequest<Result>;
