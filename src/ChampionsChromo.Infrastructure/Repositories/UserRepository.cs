using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;

namespace ChampionsChromo.Infrastructure.Repositories;

public class UserRepository(MongoDbContext context) : Repository<User>(context), IUserRepository
{
}
