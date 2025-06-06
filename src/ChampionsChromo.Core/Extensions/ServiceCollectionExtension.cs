using ChampionsChromo.Core.Services;
using ChampionsChromo.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChampionsChromo.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services.AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IStripeService, StripeService>();
    }
}
