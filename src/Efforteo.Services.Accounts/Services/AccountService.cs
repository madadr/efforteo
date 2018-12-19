using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Accounts.Domain.DTO;
using Efforteo.Services.Accounts.Domain.Models;
using Efforteo.Services.Accounts.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Efforteo.Services.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(Guid id, string email)
        {
            var account = await _repository.GetAsync(email);

            if (account != null)
            {
                throw new EfforteoException("account_already_added",
                    $"Account for email: '{email}' is already created.");
            }

            account = new Account(id, email);
            await _repository.AddAsync(account);
        }

        public async Task<AccountDto> GetAsync(string email)
        {
            var user = await _repository.GetAsync(email);
            if (user == null)
            {
                throw new EfforteoException("account_not_exists", $"Account doesn't exist.");
            }

            return _mapper.Map<AccountDto>(user);
        }

        public async Task<AccountDto> GetAsync(Guid id)
        {
            var account = await _repository.GetAsync(id);
            if (account == null)
            {
                throw new EfforteoException("account_not_exists", $"Account doesn't exist.");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            var accounts = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task UpdateAsync(AccountDto accountDto)
        {
            var account = await _repository.GetAsync(accountDto.Id);
            if (account == null)
            {
                throw new EfforteoException("account_not_exists", $"Account doesn't exist {accountDto.Id}.");
            }

            account.SetAccountData(accountDto.Email, accountDto.Name, accountDto.Location, accountDto.Weight,
                accountDto.Birthday);

            await _repository.UpdateAsync(account);
        }

        public async Task UpdateLoggedInAsync(Guid id)
        {
            var account = await _repository.GetAsync(id);
            if (account == null)
            {
                throw new EfforteoException("account_not_exists", $"Account doesn't exist.");
            }

            account.UpdateLastLoggedIn();

            await _repository.UpdateAsync(account);
        }

        public async Task RemoveAsync(Guid id)
        {
            var account = await _repository.GetAsync(id);
            if (account == null)
            {
                throw new EfforteoException("account_not_exists", $"Account doesn't exist.");
            }

            await _repository.RemoveAsync(id);
        }
    }
}