using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnection _conn;
        public UserRepository(DbConnection conn)
        {
            this._conn = conn;
        }
        
        public async Task<int> InsertUser(string userId, string email, string password)
        {
            return await _conn.ExecuteAsync("INSERT INTO User(Id, Email, Password) VALUES(@userId, @email, @password)",
                new {userId, email, password});
        }

        public async Task<int> DeleteUser(string userId)
        {
            return await _conn.ExecuteAsync("DELETE FROM USER WHERE Id = @id", new {id = userId});
        }

        public async Task<int> UpdateUser(string userId, string? email, string? password)
        {
            string sql = (email == null) ? "Update USER SET Email = @email WHERE Id = @userId" : "Update USER SET Password = @password WHERE Id = @userId";
            return await _conn.ExecuteAsync(sql, new {userId, password, email});
        }

        public async Task<UserDto> GetUser(string email, string password)
        {
            return await _conn.QuerySingleAsync<UserDto>("SELECT * FROM User WHERE Email = @email AND Password = @password", new {email, password});
        }
        
        public async Task<UserDto> GetUser(string email)
        {
            return await _conn.QuerySingleAsync<UserDto>("SELECT * FROM User WHERE Email = @email", new {email});
        }
    }
}