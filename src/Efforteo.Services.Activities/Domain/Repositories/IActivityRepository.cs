using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Activities.Domain.Models;

namespace Efforteo.Services.Activities.Domain.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity> GetAsync(Guid id);
        Task<IEnumerable<Activity>> BrowseAsync();
        Task AddAsync(Activity activity);
    }
}