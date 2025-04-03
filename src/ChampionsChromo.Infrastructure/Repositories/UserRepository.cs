using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class UserRepository(MongoDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User> GetByFirebaseIdAsync(string subject)
    {
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(u => u.FirebaseId, subject);

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
