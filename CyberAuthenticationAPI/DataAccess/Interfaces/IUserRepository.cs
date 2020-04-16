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
    }
}