using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MongoDB.Driver;

namespace Efforteo.Common.Mongo
{
    public class MongoSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;

        public MongoSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            var collections = await _database.ListCollectionsAsync().Result.ToListAsync();
            if (collections.Any())
            {
                return;
            }

            await CustomSeedAsync();
        }

        protected virtual async Task CustomSeedAsync()
        {
            await Task.CompletedTask;
        }
    }
}