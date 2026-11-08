using ChampionsChromo.Core.Enums;

namespace ChampionsChromo.Core.Entities;

public class OrderStatusChange
{
    public OrderStatus Status { get; set; }
    public DateTime At { get; set; } = DateTime.UtcNow;

    // "system", "mercadopago" or the admin's name.
    public string By { get; set; } = "system";
    public string? Note { get; set; }
}
