using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IUserAlbumRepository : IRepository<UserAlbum>
{
    Task<UserAlbum?> GetByUserIdAsync(string userId);
}
