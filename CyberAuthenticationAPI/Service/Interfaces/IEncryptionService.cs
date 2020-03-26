using System.Threading.Tasks;

namespace Service
{
    public interface IEncryptionService
    {
        Task<string> HashAsync(string str);
    }
}