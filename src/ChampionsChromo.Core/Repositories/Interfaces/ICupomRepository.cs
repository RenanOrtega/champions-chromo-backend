using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface ICupomRepository : IRepository<Cupom>
{
    Task UpdateAsync(string Id, UpdateCupomDto updateCupomDto);
}
