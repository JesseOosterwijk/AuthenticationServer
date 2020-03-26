using System;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        void InsertUser(string userId, string email, string password);
        void DeleteUser(string userId);
        void UpdateUser(string userId, string? email, string? password);
        Task<UserDto> GetUser(string email, string password);
    }
}