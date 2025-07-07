using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IOrderRepository : IRepository<OrderSummary>
{
    Task UpdateAsync(string Id, UpdateOrderDto updateOrderDto);
    Task<MetricsDto> GetDashboardMetricsAsync(int daysBack = 30);
    Task<IEnumerable<OrderSummary>> GetAllOrderedByCreatedAsync(bool descending = true);
}
