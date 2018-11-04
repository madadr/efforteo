using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Activities.Domain.Models;

namespace Efforteo.Services.Activities.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetAsync(string name);
        Task<IEnumerable<Category>> BrowseAsync();
        Task AddAsync(Category category);
    }
}