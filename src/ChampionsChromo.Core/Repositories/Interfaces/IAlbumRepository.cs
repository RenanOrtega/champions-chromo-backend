using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> FindBySchoolIdAsync(string schoolId);
}
