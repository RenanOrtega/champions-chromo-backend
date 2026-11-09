namespace ChampionsChromo.Core.Enums;

public enum PaymentMode
{
    // Order is created and closed over WhatsApp (current behaviour).
    Manual,
    // Customer pays with Pix through Mercado Pago and the order is confirmed automatically.
    MercadoPagoPix
}
