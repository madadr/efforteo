using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Activities.Domain.DTO;

namespace Efforteo.Services.Activities.Services
{
    public interface IActivityService
    {
        Task AddAsync(Guid userId, Guid id, string category, string name, string description, DateTime createdAt);
        Task<ActivityDto> GetAsync(Guid id);
        Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(Guid userId);
        Task UpdateAsync(ActivityDto activity);
        Task RemoveAsync(Guid id);
    }
}