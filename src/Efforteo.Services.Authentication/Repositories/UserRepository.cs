﻿using System;
using System.Threading.Tasks;
using Efforteo.Services.Authentication.Domain.Models;
using Efforteo.Services.Authentication.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Efforteo.Services.Authentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<User> Collection
            => _database.GetCollection<User>("users");

        public UserRepository(IMongoDatabase database)
            => _database = database;

        public async Task AddAsync(User user)
            => await Collection.InsertOneAsync(user);

        public async Task<User> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string email)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Email == email);

        public async Task UpdateAsync(User user)
            => await Collection.ReplaceOneAsync(x => x.Id == user.Id, user);

        public async Task RemoveAsync(Guid id)
            => await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}