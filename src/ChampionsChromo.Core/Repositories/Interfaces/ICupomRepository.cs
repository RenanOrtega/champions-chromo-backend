using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface ICupomRepository : IRepository<Cupom>
{
    Task UpdateAsync(string Id, UpdateCupomDto updateCupomDto);
    Task<Cupom> FindByCodeAsync(string code);

    /// <summary>Counts one use of the coupon if it still has uses left. Safe under concurrent orders.</summary>
    Task<bool> TryConsumeAsync(string id);

    /// <summary>Gives back one use (when the order could not be saved).</summary>
    Task ReleaseAsync(string id);

}
