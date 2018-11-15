using System;
using System.Threading.Tasks;
using Efforteo.Common.Auth;
using Efforteo.Services.Identity.Domain.DTO;

namespace Efforteo.Services.Identity.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        // TODO: think about updating user - different methods for different fields? If Use UserDto as parameter, then what about password?
//        Task UpdateAsync(UserDto user);

        Task<UserDto> GetAsync(string email);
        Task<UserDto> GetAsync(Guid id);
        Task<JsonWebToken> LoginAsync(string email, string password);
    }
}