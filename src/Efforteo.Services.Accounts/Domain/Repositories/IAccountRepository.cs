using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Services.Accounts.Domain.Models;

namespace Efforteo.Services.Accounts.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account);
        Task<Account> GetAsync(Guid id);
        Task<Account> GetAsync(string email);
        Task<IEnumerable<Account>> GetAllAsync();
        Task UpdateAsync(Account account);
        Task RemoveAsync(Guid id);
    }
}