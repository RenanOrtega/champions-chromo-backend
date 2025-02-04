using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;

namespace ChampionsChromo.Infrastructure.Repositories;

public class AlbumRepository(MongoDbContext context) : Repository<Album>(context), IAlbumRepository
{
}
