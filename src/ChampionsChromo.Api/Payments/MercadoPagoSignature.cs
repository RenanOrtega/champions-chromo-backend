using System.Security.Cryptography;
using System.Text;

namespace ChampionsChromo.Api.Payments;

// Validates the x-signature header Mercado Pago sends with every webhook.
// https://www.mercadopago.com.br/developers/pt/docs/checkout-api-orders/notifications
public static class MercadoPagoSignature
{
    public static bool IsValid(string? xSignature, string? xRequestId, string? dataId, string secret)
    {
        if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(xSignature))
            return false;

        string? ts = null, v1 = null;
        foreach (var part in xSignature.Split(','))
        {
            var eq = part.IndexOf('=');
            if (eq < 0) continue;

            var key = part[..eq].Trim();
            var value = part[(eq + 1)..].Trim();
            if (key == "ts") ts = value;
            else if (key == "v1") v1 = value;
        }

        if (string.IsNullOrEmpty(ts) || string.IsNullOrEmpty(v1))
            return false;

        // Manifest template: id:[data.id];request-id:[x-request-id];ts:[ts];
        // data.id must be lowercased; parts that are missing are left out.
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(dataId)) parts.Add($"id:{dataId.ToLowerInvariant()}");
        if (!string.IsNullOrEmpty(xRequestId)) parts.Add($"request-id:{xRequestId}");
        parts.Add($"ts:{ts}");
        var manifest = string.Join(";", parts) + ";";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var computed = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(manifest))).ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computed),
            Encoding.UTF8.GetBytes(v1.ToLowerInvariant()));
    }
}
