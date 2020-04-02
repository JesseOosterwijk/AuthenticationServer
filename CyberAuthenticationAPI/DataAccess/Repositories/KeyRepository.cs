using System.Data.Common;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class KeyRepository : IKeyRepository
    {
        private readonly DbConnection conn;
        public KeyRepository(DbConnection conn)
        {
            this.conn = conn;
        }
    }
}