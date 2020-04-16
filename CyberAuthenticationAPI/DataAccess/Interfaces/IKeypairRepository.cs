using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IKeypairRepository
    {
        Task<KeypairDto> GetKeypair(string userId);
        Task InsertKeypair(KeyValuePair<string, string> keyPair, string userId);
        Task DeleteKeypair(string userId);
    }
}
