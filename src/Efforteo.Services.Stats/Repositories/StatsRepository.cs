using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Policy;
using System.Threading.Tasks;
using Efforteo.Services.Stats.Domain.Models;
using Efforteo.Services.Stats.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Efforteo.Services.Stats.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<Stat> Collection
            => _database.GetCollection<Stat>("Stats");

        public StatsRepository(IMongoDatabase database)
            => _database = database;

        public async Task AddAsync(Stat Stat)
            => await Collection.InsertOneAsync(Stat);

        public async Task<Stat> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Stat>> GetUserAsync(Guid userId)
            => await Collection
                .Find(s => s.UserId == userId)
                .ToListAsync();

        public async Task<IEnumerable<Stat>> GetPeriodAsync(Guid userId, int days)
        {
            var day = DateTime.UtcNow.AddDays(1 - days);
            var dayStart = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0, DateTimeKind.Utc);

            return await Collection
                .Find(s => s.CreatedAt.CompareTo(dayStart) >= 0)
                .ToListAsync();
        }

        public async Task UpdateAsync(Stat stat)
            => await Collection.ReplaceOneAsync(x => x.Id == stat.Id, stat);

        public async Task RemoveAsync(Guid id)
            => await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}