using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class OrderRepository(MongoDbContext context) : Repository<OrderSummary>(context), IOrderRepository
{
    public async Task UpdateAsync(string Id, UpdateOrderDto updateOrderDto)
    {
        var filter = Builders<OrderSummary>.Filter.Eq(a => a.Id, Id);

        var update = Builders<OrderSummary>.Update
            .Set(a => a.UpdatedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update);
    }
}
