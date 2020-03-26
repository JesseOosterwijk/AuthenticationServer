using System;
using System.Threading.Tasks;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async void InsertUser(string userId, string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public async void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async void UpdateUser(string userId, string? email, string? password)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetUser(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}