using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class AlbumRepository(MongoDbContext context) : Repository<Album>(context), IAlbumRepository
{
    public async Task<IEnumerable<Album>> FindBySchoolIdAsync(string schoolId)
    {
        var filter = Builders<Album>.Filter.Eq(a => a.SchoolId, schoolId);
        return await _collection.Find(filter).ToListAsync();
    }
}
