using Dapper;
using DataAccess.Interfaces;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class KeypairRepository : IKeypairRepository
    {
        private readonly DbConnection _conn;

        public KeypairRepository(DbConnection conn)
        {
            this._conn = conn;
        }

        public async Task<KeypairDto> GetKeypair(string userId)
        {
            return await _conn.QuerySingleAsync<KeypairDto>("SELECT * FROM KeyPair WHERE UserId = @userId", new { userId });
        }

        public async Task InsertKeypair(KeyValuePair<string, string> keyPair, string userId)
        {
            await _conn.ExecuteAsync("INSERT INTO KeyPair (UserId, PublicKey, PrivateKey) Values (@userId, @publicKey, @privateKey)", 
                new { userId, publicKey = keyPair.Key, privateKey = keyPair.Value });
        }

        public async Task DeleteKeypair(string userId)
        {
            await _conn.ExecuteAsync("DELETE FROM KeyPair WHERE UserId = @userId", new { userId });
        }
    }
}
