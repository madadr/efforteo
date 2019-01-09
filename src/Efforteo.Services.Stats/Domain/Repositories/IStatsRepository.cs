using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Stats.Domain.Models;

namespace Efforteo.Services.Stats.Domain.Repositories
{
    public interface IStatsRepository
    {
        Task AddAsync(Stat stat);
        Task<Stat> GetAsync(Guid id);
        Task<IEnumerable<Stat>> GetUserAsync(Guid userId);
        Task UpdateAsync(Stat stat);
        Task RemoveAsync(Guid id);
    }
}