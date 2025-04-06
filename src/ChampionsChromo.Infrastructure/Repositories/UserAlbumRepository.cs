using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class UserAlbumRepository(MongoDbContext context) : Repository<UserAlbum>(context), IUserAlbumRepository
{
    public async Task<UserAlbum> AddAlbumToUser(string userId, string albumId)
    {
        var builder = Builders<UserAlbum>.Filter;
        var filter = builder.Eq(a => a.UserId, userId);

        var userAlbum = await _collection.Find(filter).FirstOrDefaultAsync();
        userAlbum.Albums.Add(new UserAlbumEntry()
        {
            AlbumId = albumId,
            OwnedA4Stickers = [],
            OwnedCommonStickers = [],
            OwnedFrameStickers = [],
            OwnedLegendStickers = [],
        });
        await _collection.ReplaceOneAsync(filter, userAlbum);

        return userAlbum;
    }

    public async Task<IEnumerable<UserAlbum>> GetByUserIdAndAlbumId(string albumId, string userId)
    {
        var builder = Builders<UserAlbum>.Filter;
        var filter = builder.ElemMatch(a => a.Albums, album => album.AlbumId == albumId) &
            builder.Eq(a => a.UserId, userId);

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<UserAlbum?> GetByUserIdAsync(string userId)
    {
        var builder = Builders<UserAlbum>.Filter;
        var filter = builder.Eq(a => a.UserId, userId);

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
