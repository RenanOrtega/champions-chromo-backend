using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IUserAlbumRepository : IRepository<UserAlbum>
{
    Task<UserAlbum> AddAlbumToUser(string userId, string albumId);
    Task<UserAlbum?> GetByUserIdAsync(string userId);
}
