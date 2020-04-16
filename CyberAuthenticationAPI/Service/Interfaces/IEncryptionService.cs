using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEncryptionService
    {
        public Task<KeyValuePair<byte[], byte[]>> GenerateKeyPair();
        
    }
}