using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class UserRepository(MongoDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _collection
            .Find(u => u.Username == username && u.IsActive)
            .FirstOrDefaultAsync();
    }

    public new async Task<User?> GetByIdAsync(string id)
    {
        return await _collection
            .Find(u => u.Id == id && u.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        await _collection.InsertOneAsync(user);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        return user;
    }

    public new async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.UpdateOneAsync(
            u => u.Id == id,
            Builders<User>.Update.Set(u => u.IsActive, false)
        );
        return result.ModifiedCount > 0;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        var count = await _collection
            .CountDocumentsAsync(u => u.Username == username && u.IsActive);
        return count > 0;
    }
}
