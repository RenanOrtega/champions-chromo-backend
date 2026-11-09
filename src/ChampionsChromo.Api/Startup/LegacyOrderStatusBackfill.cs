using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Repositories.Interfaces;

namespace ChampionsChromo.Api.Startup;

// One-off migration: orders created before status tracking have no Status field and would show up as "Novo".
// They were all handled through the manual WhatsApp flow, so they are marked as "Enviado".
// The filter only matches documents without the field, so later runs do nothing.
public class LegacyOrderStatusBackfill(IServiceScopeFactory scopeFactory, ILogger<LegacyOrderStatusBackfill> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var orders = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

            var updated = await orders.BackfillLegacyStatusAsync(new OrderStatusChange
            {
                Status = OrderStatus.Shipped,
                At = DateTime.UtcNow,
                By = "system",
                Note = "Pedido anterior ao controle de status",
            });

            if (updated > 0)
                logger.LogInformation("Marked {Count} legacy orders as Shipped", updated);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Legacy order status backfill failed; it will run again on the next start");
        }
    }
}
