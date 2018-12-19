using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Activities.Domain.Models;
using Efforteo.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Efforteo.Services.Activities.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<Category> Collection
            => _database.GetCollection<Category>("categories");

        public CategoryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Category>> BrowseAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task<Category> GetAsync(string name)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Name == name);

        public async Task AddAsync(Category category)
            => await Collection
                .InsertOneAsync(category);
    }
}