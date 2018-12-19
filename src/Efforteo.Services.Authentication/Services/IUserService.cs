using System;
using System.Threading.Tasks;
using Efforteo.Common.Auth;
using Efforteo.Services.Authentication.Domain.DTO;

namespace Efforteo.Services.Authentication.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        Task<UserDto> GetAsync(string email);
        Task<UserDto> GetAsync(Guid id);
        Task<JsonWebToken> LoginAsync(string email, string password);
        Task ChangePassword(Guid commandUserId, string commandOldPassword, string commandNewPassword);
        Task RemoveAsync(Guid id);
    }
}