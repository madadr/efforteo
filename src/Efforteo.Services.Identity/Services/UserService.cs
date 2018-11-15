using System;
using System.Threading.Tasks;
using AutoMapper;
using Efforteo.Common.Auth;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Identity.Domain.DTO;
using Efforteo.Services.Identity.Domain.Models;
using Efforteo.Services.Identity.Domain.Repositories;
using Efforteo.Services.Identity.Domain.Services;

namespace Efforteo.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IEncrypter _encrypter;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IEncrypter encrypter, IJwtHandler jwtHandler, IMapper mapper)
        {
            _repository = repository;
            _encrypter = encrypter;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await _repository.GetAsync(email);
            if (user != null)
            {
                throw new EfforteoException("email_already_taken", $"Email: '{email}' is already in use.");
            }
            user = new User(email, name);
            user.SetPassword(password, _encrypter);
            await _repository.AddAsync(user);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user = await _repository.GetAsync(email);
            if (user == null)
            {
                throw new EfforteoException("invalid_credentials", $"Invalid credentials.");
            }
            if (!user.ValidatePassword(password, _encrypter))
            {
                throw new EfforteoException("invalid_credentials", $"Invalid credentials.");
            }

            return _jwtHandler.Create(user.Id);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _repository.GetAsync(id);
            if (user == null)
            {
                throw new EfforteoException("user_not_exists", $"User doesn't exist.");
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _repository.GetAsync(email);
            if (user == null)
            {
                throw new EfforteoException("user_not_exists", $"User doesn't exist.");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}