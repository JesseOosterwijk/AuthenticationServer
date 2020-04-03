using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class SaltMine : ISaltRepository
    {
        private readonly DbConnection _conn;
        public SaltMine(DbConnection conn)
        {
            this._conn = conn;
        }
        
        public async Task<int> InsertSalt(string userId, byte[] salt)
        {
            return await _conn.ExecuteAsync("INSERT INTO Salt(Id, Salt) VALUES(@userId, @salt)", new {userId, salt});
        }

        public async Task<int> DeleteSalt(string userId)
        {
            return await _conn.ExecuteAsync("DELETE FROM Salt WHERE Id = @userId", new {userId});
        }

        public async Task<int> UpdateSalt(string userId, byte[] salt)
        {
            return await _conn.ExecuteAsync("UPDATE Salt SET Salt = @salt WHERE Id = @userId", new {userId, salt});
        }

        public async Task<byte[]> GetSalt(string userId)
        {
            return (await _conn.QueryFirstAsync<SaltDto>("SELECT Salt FROM Salt WHERE Id = @userId", new {userId})).Salt;
        }
    }

    class SaltDto
    {
        public byte[] Salt { get; set; }
    }
}