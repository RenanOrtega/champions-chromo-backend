using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IUserAlbumRepository : IRepository<UserAlbum>
{
    Task<IEnumerable<UserAlbum>> FindByAlbumIdAsync(string albumId, string userId);
}
