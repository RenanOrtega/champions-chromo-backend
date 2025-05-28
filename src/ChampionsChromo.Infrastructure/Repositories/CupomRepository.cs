using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class CupomRepository(MongoDbContext context) : Repository<Cupom>(context), ICupomRepository
{
    public async Task UpdateAsync(string Id, UpdateCupomDto updateAlbumDto)
    {
        var filter = Builders<Cupom>.Filter.Eq(a => a.Id, Id);

        var update = Builders<Cupom>.Update
            .Set(c => c.Type, updateAlbumDto.Type)
            .Set(c => c.Value, updateAlbumDto.Value)
            .Set(c => c.UsedCount, updateAlbumDto.UsageLimit)
            .Set(c => c.ExpiresAt, updateAlbumDto.ExpiresAt)
            .Set(c => c.MinPurchaseValue, updateAlbumDto.MinPurchaseValue)
            .Set(c => c.IsActive, updateAlbumDto.IsActive);

        await _collection.UpdateOneAsync(filter, update);
    }
}
