using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IOrderRepository : IRepository<OrderSummary>
{
    Task UpdateAsync(string Id, UpdateOrderDto updateOrderDto);
    Task<MetricsDto> GetDashboardMetricsAsync(DateTime? from = null, DateTime? to = null);
    Task<IEnumerable<OrderSummary>> GetAllOrderedByCreatedAsync(bool descending = true, bool includeArchived = false);

    // Sets ArchivedAt (archive) or clears it (unarchive) on the given orders. Returns how many were changed.
    Task<long> SetArchivedAsync(IEnumerable<string> ids, DateTime? archivedAt);
    Task SetPaymentAsync(string id, OrderPayment payment, OrderStatusChange? statusChange = null);
    Task SetStatusAsync(string id, OrderStatusChange statusChange);

    // Gives a status to the orders created before status tracking existed. Returns how many were updated.
    Task<long> BackfillLegacyStatusAsync(OrderStatusChange statusChange);
}
