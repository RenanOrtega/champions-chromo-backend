using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Options;

// Feature toggle: Payments__Mode = Manual (default) | MercadoPagoPix.
public class PaymentOptions
{
    public const string SectionName = "Payments";

    public PaymentMode Mode { get; set; } = PaymentMode.Manual;
}
