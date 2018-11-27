using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Activities.Domain.Models;
using Efforteo.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Efforteo.Services.Activities.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<Activity> Collection
            => _database.GetCollection<Activity>("activities");

        public ActivityRepository(IMongoDatabase database)
            => _database = database;

        public async Task<IEnumerable<Activity>> BrowseAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task<Activity> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Activity activity)
            => await Collection
                .InsertOneAsync(activity);
    }
}