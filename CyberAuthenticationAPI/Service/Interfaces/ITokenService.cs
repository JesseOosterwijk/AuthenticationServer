using DataAccess;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(UserDto user, string _privKey);
        Task<bool> VerifyToken(string token);
    }
}