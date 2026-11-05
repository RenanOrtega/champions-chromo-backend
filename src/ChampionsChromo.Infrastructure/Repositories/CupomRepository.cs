using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class CupomRepository(MongoDbContext context) : Repository<Cupom>(context), ICupomRepository
{
    public async Task<Cupom> FindByCodeAsync(string code)
    {
        return await _collection.Find(c => c.Code == code && c.IsActive).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string Id, UpdateCupomDto updateAlbumDto)
    {
        var filter = Builders<Cupom>.Filter.Eq(a => a.Id, Id);

        var update = Builders<Cupom>.Update
            .Set(c => c.Type, updateAlbumDto.Type)
            .Set(c => c.Value, updateAlbumDto.Value)
            .Set(c => c.UsageLimit, updateAlbumDto.UsageLimit)
            .Set(c => c.ExpiresAt, updateAlbumDto.ExpiresAt)
            .Set(c => c.MinPurchaseValue, updateAlbumDto.MinPurchaseValue)
            .Set(c => c.IsActive, updateAlbumDto.IsActive);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<bool> TryConsumeAsync(string id)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var coupon = await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (coupon is null)
                return false;

            if (coupon.UsageLimit > 0 && coupon.UsedCount >= coupon.UsageLimit)
                return false;

            // Only increments if nobody else used the coupon since we read it.
            var result = await _collection.UpdateOneAsync(
                c => c.Id == id && c.UsedCount == coupon.UsedCount,
                Builders<Cupom>.Update.Inc(c => c.UsedCount, 1));

            if (result.ModifiedCount > 0)
                return true;
        }

        return false;
    }

    public async Task ReleaseAsync(string id)
    {
        await _collection.UpdateOneAsync(
            c => c.Id == id && c.UsedCount > 0,
            Builders<Cupom>.Update.Inc(c => c.UsedCount, -1));
    }
}
