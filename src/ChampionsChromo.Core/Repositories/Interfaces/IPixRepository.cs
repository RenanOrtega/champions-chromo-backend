using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IPixRepository : IRepository<PixOrder>
{
    Task<PixOrder> FindByIntegrationId(string integrationId);
}
