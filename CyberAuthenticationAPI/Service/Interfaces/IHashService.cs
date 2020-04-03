using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IHashService
    {
        Task<string> HashAsync(string str, byte[] salt);
        Task<string> HashAsync(string str, out byte[] salt);
    }
}