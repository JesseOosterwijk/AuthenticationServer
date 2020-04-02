using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        void AddUser(string email, string password);
        Task<string> Login(string email, string password);
        void DeleteUser(string userId, string password);
    }
}