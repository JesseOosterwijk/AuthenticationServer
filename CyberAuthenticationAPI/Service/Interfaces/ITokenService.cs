using DataAccess;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(UserDto user);
        bool VerifyToken(string token);
    }
}