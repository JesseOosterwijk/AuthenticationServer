using System.Data.Common;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class KeyRepository : IKeyRepository
    {
        private readonly DbConnection _conn;
        public KeyRepository(DbConnection conn)
        {
            this._conn = conn;
        }
    }
}