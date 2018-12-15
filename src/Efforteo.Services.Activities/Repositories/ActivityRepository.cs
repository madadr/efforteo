using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddAsync(Activity activity)
            => await Collection
                .InsertOneAsync(activity);

        public async Task<Activity> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Activity>> GetAllAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task<IEnumerable<Activity>> GetUserActivitiesAsync(Guid userId)
            => await Collection
                .Find(a => a.UserId == userId)
                .ToListAsync();

        public async Task UpdateAsync(Activity activity)
            => await Collection.ReplaceOneAsync(x => x.Id == activity.Id, activity);

        public async Task RemoveAsync(Guid id)
            => await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}