using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByFirebaseIdAsync(string subject);
}