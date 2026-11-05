using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Application.Cupoms;

public static class CouponRules
{
    /// <summary>Returns why the coupon can't be used right now, or null when it can.</summary>
    public static string? GetUnavailableReason(Cupom coupon, DateTime nowUtc)
    {
        if (!coupon.IsActive)
            return "Cupom inativo.";

        // Coupons created without an expiration date keep the default (year 1): no expiration.
        if (coupon.ExpiresAt.Year > 1 && coupon.ExpiresAt < nowUtc)
            return "Cupom expirado.";

        if (coupon.UsageLimit > 0 && coupon.UsedCount >= coupon.UsageLimit)
            return "Cupom esgotado.";

        return null;
    }

    public static decimal CalculateDiscount(Cupom coupon, decimal subtotal)
    {
        var discount = coupon.Type switch
        {
            CupomType.Percent => subtotal * coupon.Value / 100m,
            CupomType.Fixed => Math.Min(coupon.Value, subtotal),
            _ => 0m
        };

        return Math.Round(Math.Max(0m, discount), 2);
    }
}
