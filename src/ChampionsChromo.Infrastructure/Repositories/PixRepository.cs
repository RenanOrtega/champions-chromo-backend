using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class PixRepository(MongoDbContext context) : Repository<PixOrder>(context), IPixRepository
{
    public async Task<PixOrder> FindByIntegrationId(string integrationId)
    {
        var filter = Builders<PixOrder>.Filter.Eq(a => a.IntegrationId, integrationId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
