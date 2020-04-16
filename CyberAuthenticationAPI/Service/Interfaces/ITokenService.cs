using DataAccess;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(UserDto user);
        bool VerifyToken(string token);
    }
}