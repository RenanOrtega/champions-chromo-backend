using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Payments.Commands.ProcessMercadoPagoNotification;

// The notification body is never trusted: only the Mercado Pago order id is used,
// and the order is fetched again from the Mercado Pago API with our access token.
public record ProcessMercadoPagoNotificationCommand(string ProviderOrderId) : IRequest<Result>;
