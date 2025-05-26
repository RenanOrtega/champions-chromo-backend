using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface ISchoolRepository : IRepository<School>
{
    Task UpdateAsync(string Id, UpdateSchoolDto updateSchoolDto);
}
