using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Efforteo.Common.Auth;
using Efforteo.Services.Accounts.Domain.DTO;
using Microsoft.Extensions.Primitives;

namespace Efforteo.Services.Accounts.Services
{
    public interface IAccountService
    {
        Task AddAsync(Guid id, string email);
        Task<AccountDto> GetAsync(string email);
        Task<AccountDto> GetAsync(Guid id);
        Task<IEnumerable<AccountDto>> GetAllAsync();
        Task UpdateAsync(AccountDto account);
        Task UpdateLoggedInAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}