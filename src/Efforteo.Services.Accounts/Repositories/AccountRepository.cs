using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Policy;
using System.Threading.Tasks;
using Efforteo.Services.Accounts.Domain.Models;
using Efforteo.Services.Accounts.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Efforteo.Services.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<Account> Collection
            => _database.GetCollection<Account>("accounts");

        public AccountRepository(IMongoDatabase database)
            => _database = database;

        public async Task AddAsync(Account account)
            => await Collection.InsertOneAsync(account);

        public async Task<Account> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Account> GetAsync(string email)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Email == email);

        public async Task<IEnumerable<Account>> GetAllAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task UpdateAsync(Account account)
            => await Collection.ReplaceOneAsync(x => x.Id == account.Id, account);

        public async Task RemoveAsync(Guid id)
            => await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}