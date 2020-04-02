using DataAccess;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(UserDto user);
        string VerifyToken(string token);
    }
}