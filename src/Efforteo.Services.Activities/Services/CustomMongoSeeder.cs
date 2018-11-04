using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Mongo;
using Efforteo.Services.Activities.Domain.Models;
using Efforteo.Services.Activities.Domain.Repositories;
using MongoDB.Driver;

namespace Efforteo.Services.Activities.Services
{
    public class CustomMongoSeeder : MongoSeeder
    {
        private readonly ICategoryRepository _categoryRepository;

        public CustomMongoSeeder(IMongoDatabase database, ICategoryRepository categoryRepository) : base(database)
        {
            _categoryRepository = categoryRepository;
        }

        protected override async Task CustomSeedAsync()
        {
            var categories = new List<string>
            {
                "ride",
                "run",
                "swim"
            };
            await Task.WhenAll(categories.Select(x => _categoryRepository.AddAsync(new Category(x))));
        }
    }
}