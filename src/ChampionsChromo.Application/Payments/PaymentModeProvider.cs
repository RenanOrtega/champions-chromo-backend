using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Options;
using Microsoft.Extensions.Options;

namespace ChampionsChromo.Application.Payments;

public interface IPaymentModeProvider
{
    PaymentMode Current { get; }
}

// Pix is only active when the toggle is on AND Mercado Pago is configured;
// a missing access token falls back to the manual flow instead of breaking checkout.
public class PaymentModeProvider(
    IOptionsMonitor<PaymentOptions> paymentOptions,
    IOptionsMonitor<MercadoPagoOptions> mercadoPagoOptions) : IPaymentModeProvider
{
    public PaymentMode Current =>
        paymentOptions.CurrentValue.Mode == PaymentMode.MercadoPagoPix
        && !string.IsNullOrWhiteSpace(mercadoPagoOptions.CurrentValue.AccessToken)
            ? PaymentMode.MercadoPagoPix
            : PaymentMode.Manual;
}
