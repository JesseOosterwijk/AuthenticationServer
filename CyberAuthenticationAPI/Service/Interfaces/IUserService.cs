using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task AddUser(string email, string password);
        Task<string> Login(string email, string password);
        Task DeleteUser(string userId, string password);
    }
}