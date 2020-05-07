using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Service.Interfaces;

namespace Service.Implementations
{
    public class RsaEncryptionService : IEncryptionService
    {
        public KeyValuePair<string, string> GenerateKeyXml()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048);
            string _privKey = provider.ToXmlString(true);
            string _pubKey = provider.ToXmlString(false);
            return new KeyValuePair<string, string>(_pubKey, _privKey);
        }
    }
}