using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> FindBySchoolIdAsync(string schoolId);
    Task UpdateAsync(string Id, UpdateAlbumDto updateAlbumDto);
}
