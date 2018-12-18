using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Stats.Domain.DTO;
using Efforteo.Services.Stats.Domain.Models;
using Efforteo.Services.Stats.Domain.Repositories;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace Efforteo.Services.Stats.Services
{
    public class StatService : IStatService
    {
        private readonly IStatsRepository _repository;
        private readonly IMapper _mapper;

        public StatService(IStatsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(Guid id, Guid userId, string category, float distance, long time, DateTime createdAt)
        {
            var stat = await _repository.GetAsync(id);
            
            if (stat != null)
            {
            throw new EfforteoException("stat_already_added", $"Stat is already created.");
            }
            
            stat = new Stat(id, userId, category, distance, time, createdAt);
            await _repository.AddAsync(stat);
        }

        public async Task<StatDto> GetAsync(Guid id)
        {
            var stat = await _repository.GetAsync(id);
            if (stat == null)
            {
            throw new EfforteoException("stat_not_exists", $"Stat doesn't exist.");
            }
            
            return _mapper.Map<StatDto>(stat);
        }

        public async Task<IEnumerable<CategoryTotalStatsDto>> GetTotalAsync(Guid userId)
        {
            var userStats = await _repository.GetUserAsync(userId);
            if (!userStats.Any())
            {
                return null;
            }

            var userStatsByCategory = userStats.GroupBy(stat => stat.Category.ToLowerInvariant());
            List<CategoryTotalStats> totalStats = new List<CategoryTotalStats>();
            foreach (var categoryStats in userStatsByCategory)
            {
                totalStats.Add(new CategoryTotalStats(categoryStats));
            }

            return _mapper.Map<IEnumerable<CategoryTotalStatsDto>>(totalStats);
        }

        public async Task<IEnumerable<CategoryPeriodicStatsDto>> GetPeriodAsync(Guid userId, int days)
        {
            var userStats = await _repository.GetUserAsync(userId);
            if (!userStats.Any())
            {
                return null;
            }

            if (days <= 0)
            {
                throw new EfforteoException("negative_days", "Period cannot have negative days");
            }

            var userStatsByCategory = userStats.GroupBy(stat => stat.Category.ToLowerInvariant());
            List <CategoryPeriodicStats> periodStats = new List<CategoryPeriodicStats>();
            foreach (var categoryStats in userStatsByCategory)
            {
                periodStats.Add(new CategoryPeriodicStats(categoryStats, days));
            }

            return _mapper.Map<IEnumerable<CategoryPeriodicStatsDto>>(periodStats);
        }

        public async Task<IEnumerable<CategoryDetailedStatsDto>> GetDetailedStats(Guid userId)
        {
            var userStats = await _repository.GetUserAsync(userId);
            if (!userStats.Any())
            {
                return null;
            }

            var userStatsByCategory = userStats.GroupBy(stat => stat.Category.ToLowerInvariant());
            List<CategoryDetailedStats> detailedStats = new List<CategoryDetailedStats>();
            foreach (var categoryStats in userStatsByCategory)
            {
                detailedStats.Add(new CategoryDetailedStats(categoryStats));
            }

            return _mapper.Map<IEnumerable<CategoryDetailedStatsDto>>(detailedStats);
        }

        public async Task UpdateAsync(StatDto statDto)
        {
            var stat = await _repository.GetAsync(statDto.Id);
            if (stat == null)
            {
               throw new EfforteoException("stat_not_exists", $"Stat doesn't exist {statDto.Id}.");
            }
                        
            stat.SetStatData(statDto.Category, statDto.Distance, statDto.Time, statDto.CreatedAt);
            
            await _repository.UpdateAsync(stat);
        }

        public async Task RemoveAsync(Guid id)
        {
            var stat = await _repository.GetAsync(id);
            if (stat == null)
            {
                throw new EfforteoException("stat_not_exists", $"Stat doesn't exist.");
            }
            
            await _repository.RemoveAsync(id);
        }
    }
}
