using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByRefreshToken(string refreshToken);
    new Task<User?> GetByIdAsync(string id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    new Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string username);
}