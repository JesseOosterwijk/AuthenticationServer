using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        public void AddUser(string email, string password);
        Task<string> Login(string email, string password);
    }
}