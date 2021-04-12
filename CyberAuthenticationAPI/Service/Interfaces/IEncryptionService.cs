using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IEncryptionService
    {
        KeyValuePair<string, string> GenerateKeyXml();

    }
}