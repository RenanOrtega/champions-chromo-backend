using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using ChampionsChromo.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ChampionsChromo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        
        services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IAlbumRepository, AlbumRepository>()
            .AddScoped<ISchoolRepository, SchoolRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserAlbumRepository, UserAlbumRepository>()
            .AddScoped<IPixRepository, PixRepository>();

        return services;
    }
}
