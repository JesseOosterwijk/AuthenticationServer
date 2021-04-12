using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<int> InsertUser(string userId, string email, string password);
        Task<int> DeleteUser(string userId);
        Task<int> UpdateUser(string userId, string? email, string? password);
        Task<UserDto> GetUser(string email, string password);
        Task<UserDto> GetUser(string email);
        Task InsertRefreshToken(string userId, string token);
        Task<string> GetRefreshToken(string userId);
        Task<int> UpdateRefreshToken(string userId, string token);
        Task<UserDto> GetUserById(string userId);
        Task<int> DeleteRefreshToken(string userId);
    }
}