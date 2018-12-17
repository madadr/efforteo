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

        public async Task<IEnumerable<CategoryTotalDto>> GetTotalAsync(Guid userId)
        {
            var userStats = await _repository.GetUserAsync(userId);
            if (!userStats.Any())
            {
                return null;
            }

            var userStatsByCategory = userStats.GroupBy(stat => stat.Category.ToLowerInvariant());
            List<CategoryTotal> totalStats = new List<CategoryTotal>();
            foreach (var categoryStats in userStatsByCategory)
            {
                totalStats.Add(new CategoryTotal(categoryStats));
            }

            return _mapper.Map<IEnumerable<CategoryTotalDto>>(totalStats);
        }

        public async Task<IEnumerable<CategoryDetailsDto>> GetPeriodAsync(Guid userId, int days)
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
            List <CategoryDetails> periodStats = new List<CategoryDetails>();
            foreach (var categoryStats in userStatsByCategory)
            {
                periodStats.Add(new CategoryDetails(categoryStats, days));
            }

            return _mapper.Map<IEnumerable<CategoryDetailsDto>>(periodStats);
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
