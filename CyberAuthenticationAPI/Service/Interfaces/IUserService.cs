using CyberAuthenticationAPI.Response;
using Service.Response;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task AddUser(string email, string password);
        Task<TokenResponse> Login(string email, string password);
        Task DeleteUser(string userId, string password);
        Task<TokenResponse> Refresh(TokenRequest response);
    }
}