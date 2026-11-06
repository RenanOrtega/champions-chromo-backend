using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IOrderRepository : IRepository<OrderSummary>
{
    Task UpdateAsync(string Id, UpdateOrderDto updateOrderDto);
    Task<MetricsDto> GetDashboardMetricsAsync(DateTime? from = null, DateTime? to = null);
    Task<IEnumerable<OrderSummary>> GetAllOrderedByCreatedAsync(bool descending = true);
}
