using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Activities.Domain.Models;

namespace Efforteo.Services.Activities.Domain.Repositories
{
    public interface IActivityRepository
    {
        Task AddAsync(Activity activity);
        Task<Activity> GetAsync(Guid id);
        Task<IEnumerable<Activity>> GetUserActivitiesAsync(Guid userId);
        Task UpdateAsync(Activity activity);
        Task RemoveAsync(Guid id);
    }
}