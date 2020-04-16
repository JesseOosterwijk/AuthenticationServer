using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Implementations
{
    public class RsaEncryptionService : IEncryptionService
    {
        public Task<KeyValuePair<byte[], byte[]>> GenerateKeyPair()
        {
            return Task.Run(() =>
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
                byte[] publicKey = rsa.ExportRSAPublicKey();
                byte[] privateKey = rsa.ExportRSAPrivateKey(); return new KeyValuePair<byte[], byte[]>(publicKey, privateKey);
            });
        }
    }
}