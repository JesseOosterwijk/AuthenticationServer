using System.Collections.Generic;
using System.Security.Cryptography;

namespace Service.Interfaces
{
    public interface IEncryptionService
    {
        KeyValuePair<string, string> GenerateKeyXml();

    }
}