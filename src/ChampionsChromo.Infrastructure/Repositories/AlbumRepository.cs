using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
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

    public async Task UpdateAsync(string Id, UpdateAlbumDto updateAlbumDto)
    {
        var filter = Builders<Album>.Filter.Eq(a => a.Id, Id);

        var update = Builders<Album>.Update
            .Set(a => a.Name, updateAlbumDto.Name)
            .Set(a => a.CoverImage, updateAlbumDto.CoverImage)
            .Set(a => a.HasA4, updateAlbumDto.HasA4)
            .Set(a => a.HasCommon, updateAlbumDto.HasCommon)
            .Set(a => a.HasLegend, updateAlbumDto.HasLegend)
            .Set(a => a.TotalStickers, updateAlbumDto.TotalStickers)
            .Set(a => a.UpdatedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateImageAsync(string Id, string imageUrl)
    {
        var filter = Builders<Album>.Filter.Eq(a => a.Id, Id);
        var update = Builders<Album>.Update.Set(a => a.CoverImage, imageUrl);
        await _collection.UpdateOneAsync(filter, update);
    }
}
