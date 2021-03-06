﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Common.Auth;
using Efforteo.Services.Stats.Domain.DTO;
using Microsoft.Extensions.Primitives;

namespace Efforteo.Services.Stats.Services
{
    public interface IStatService
    {
        Task AddAsync(Guid id, Guid userId, string category, float distance, long time, DateTime createdAt);
        Task<StatDto> GetAsync(Guid id);
        Task<IEnumerable<CategoryTotalStatsDto>> GetTotalAsync(Guid userId);
        Task<IEnumerable<CategoryPeriodicStatsDto>> GetPeriodAsync(Guid userId, int days);
        Task<IEnumerable<CategoryDetailedStatsDto>> GetDetailedStats(Guid userId);
        Task UpdateAsync(StatDto stat);
        Task RemoveAsync(Guid id);
    }
}