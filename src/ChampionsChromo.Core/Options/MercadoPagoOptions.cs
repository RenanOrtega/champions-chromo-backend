namespace ChampionsChromo.Core.Options;

// Values come from environment variables / Fly secrets (MercadoPago__AccessToken, MercadoPago__WebhookSecret).
// Never commit real values to appsettings.json.
public class MercadoPagoOptions
{
    public const string SectionName = "MercadoPago";

    public string AccessToken { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = "https://api.mercadopago.com";
    public int PixExpirationMinutes { get; set; } = 30;
}
