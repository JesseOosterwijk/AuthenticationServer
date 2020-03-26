using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ISaltRepository
    {
        void InsertSalt(string userId, byte[] salt);
        void DeleteSalt(string userId);
        void UpdateSalt(string userId, byte[] salt);
        Task<string> GetSalt(string userId);
    }
}