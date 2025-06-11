using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class SchoolRepository(MongoDbContext context) : Repository<School>(context), ISchoolRepository
{
    public async Task UpdateAsync(string Id, UpdateSchoolDto updateSchoolDto)
    {
        var filter = Builders<School>.Filter.Eq(a => a.Id, Id);

        var update = Builders<School>.Update
            .Set(a => a.Name, updateSchoolDto.Name)
            .Set(a => a.State, updateSchoolDto.State)
            .Set(a => a.City, updateSchoolDto.City)
            .Set(a => a.Warning, updateSchoolDto.Warning)
            .Set(a => a.BgWarningColor, updateSchoolDto.BgWarningColor)
            .Set(a => a.ShippingCost, updateSchoolDto.ShippingCost);

        await _collection.UpdateOneAsync(filter, update);
    }
}
