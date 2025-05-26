using System.Linq.Expressions;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class Repository<T>(MongoDbContext context) : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection = context.GetCollection<T>(typeof(T).Name.ToLower());

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task<long> CountAsync()
    {
        return await _collection.CountDocumentsAsync(_ => true);
    }
}
