﻿using System.Linq.Expressions;

namespace ChampionsChromo.Core.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<T> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter);
    Task AddAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
    Task<long> CountAsync();
}
