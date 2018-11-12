using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Efforteo.Services.Identity.Domain.Models;
using Efforteo.Services.Identity.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Efforteo.Services.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<User> Collection
            => _database.GetCollection<User>("users");

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<User> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string email)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Email == email);

        public async Task AddAsync(User user)
            => await Collection.InsertOneAsync(user);
    }
}