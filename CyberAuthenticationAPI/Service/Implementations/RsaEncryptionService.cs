using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Implementations
{
    public class RsaEncryptionService : IEncryptionService
    {
        /*public Task<KeyValuePair<string, string>> GenerateKeyPair()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ExportRSAPrivateKey();
        }*/
        public Task<KeyValuePair<string, string>> GenerateKeyPair()
        {
            throw new System.NotImplementedException();
        }
    }
}