using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Clients.Interfaces;

public interface IAbacatePayClient
{
    Task<GeneratePixAbacatePayResponse?> GeneratePix(string endpoint, GeneratePixAbacatePayRequest generatePixAbacatePayRequest, CancellationToken cancellationToken = default);
}
