using System.Threading.Tasks;
using Efforteo.Common.Auth;

namespace Efforteo.Services.Identity.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        Task<JsonWebToken> LoginAsync(string email, string password);
    }
}