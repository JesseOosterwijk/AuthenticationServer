using System.Collections.Generic;
using System.Security.Cryptography;

namespace Service.Interfaces
{
    public interface IEncryptionService
    {
        RSAParameters ParseXmlString(string xml);
        KeyValuePair<string, string> GenerateKeyXml();

    }
}