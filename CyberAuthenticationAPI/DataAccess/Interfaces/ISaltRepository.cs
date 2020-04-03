using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ISaltRepository
    {
        Task<int> InsertSalt(string userId, byte[] salt);
        Task<int> DeleteSalt(string userId);
        Task<int> UpdateSalt(string userId, byte[] salt);
        Task<byte[]> GetSalt(string userId);
    }
}