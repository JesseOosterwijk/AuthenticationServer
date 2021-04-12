using Org.BouncyCastle.Crypto;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Service.Implementations
{
    public class StringEncryptionService : IStringEncryptionService
    {
        private readonly string _encryptionKey;
        public StringEncryptionService(string encryptionKey)
        {
            this._encryptionKey = encryptionKey;
        }

        public string EncryptString(string toEncrypt)
        {

            byte[] clearBytes = Encoding.Unicode.GetBytes(toEncrypt);
            using(Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using(MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    toEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return toEncrypt;
        }

        public string DecryptString(string toDecrypt)
        {
            toDecrypt = toDecrypt.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(toDecrypt);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    toDecrypt = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return toDecrypt;
        }

        
    }
}
