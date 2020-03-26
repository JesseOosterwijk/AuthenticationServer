using System.Threading.Tasks;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class SaltMine : ISaltRepository
    {
        public async void InsertSalt(string userId, byte[] salt)
        {
            throw new System.NotImplementedException();
        }

        public async void DeleteSalt(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async void UpdateSalt(string userId, byte[] salt)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetSalt(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}