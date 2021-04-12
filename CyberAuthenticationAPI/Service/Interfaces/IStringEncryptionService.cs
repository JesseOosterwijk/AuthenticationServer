using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IStringEncryptionService
    {
        public string EncryptString(string toEncrypt);

        public string DecryptString(string toDecrypt);
    }
}
