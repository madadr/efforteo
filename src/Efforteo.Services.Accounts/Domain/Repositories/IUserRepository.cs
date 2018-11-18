using System;
using System.Threading.Tasks;
using Efforteo.Services.Accounts.Domain.Models;

namespace Efforteo.Services.Accounts.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task UpdateAsync(User user);
        Task RemoveAsync(Guid id);
    }
}