using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class SaltMine : ISaltRepository
    {
        private readonly DbConnection conn;
        public SaltMine(DbConnection conn)
        {
            this.conn = conn;
        }
        
        public async Task<int> InsertSalt(string userId, byte[] salt)
        {
            return await conn.ExecuteAsync("INSERT INTO Salt(UserId, Salt) VALUES(@userId, salt)", new {userId, salt});
        }

        public async Task<int> DeleteSalt(string userId)
        {
            return await conn.ExecuteAsync("DELETE FROM Salt WHERE UserId = @userId", new {userId});
        }

        public async Task<int> UpdateSalt(string userId, byte[] salt)
        {
            return await conn.ExecuteAsync("UPDATE Salt SET Salt = @salt WHERE UserId = @userId", new {userId, salt});
        }

        public async Task<string> GetSalt(string userId)
        {
            return await conn.QueryFirstAsync<string>("SELECT Salt FROM Salt WHERE UserId = @userId", new {userId});
        }
    }
}