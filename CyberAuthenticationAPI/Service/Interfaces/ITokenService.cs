using DataAccess;

namespace Service
{
    public interface ITokenService
    {
        public string GenerateToken(UserDto user);
        string VerifyToken(string token);
    }
}