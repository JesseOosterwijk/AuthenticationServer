using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Service.Interfaces;

namespace Service.Implementations
{
    //https://github.com/kmaragon/Konscious.Security.Cryptography
    public class Argon2Service : IHashService
    {
        private readonly int _hashIterations;
        private readonly int _parallelism;
        private readonly int _saltLength;
        private readonly int _memorySize;
        private readonly byte[]? _pepper;
        private readonly int _hashLength;

        public Argon2Service(int hashIterations, int parallelism, int memorySze, int saltLength = 32,
            byte[]? pepper = null, int length = 128)
        {
            this._hashIterations = hashIterations;
            this._parallelism = parallelism;
            this._saltLength = saltLength;
            this._memorySize = memorySze;
            this._pepper = pepper;
            this._hashLength = length;
        }

        public Task<string> HashAsync(string str, out byte[] salt)
        {
            byte[] generatedSalt = new byte[_saltLength];
            salt = generatedSalt;
            using RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            random.GetNonZeroBytes(salt);
            return HashAsync(str, generatedSalt);
        }

        public async Task<string> HashAsync(string str, byte[] salt)
        {
            Argon2i argon2 = new Argon2i(Encoding.UTF8.GetBytes(str));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = _parallelism;
            argon2.Iterations = _hashIterations;
            argon2.MemorySize = _memorySize;

            if (_pepper != null)
            {
                argon2.AssociatedData = _pepper;
            }

            string hashedPass = Encoding.ASCII.GetString(await argon2.GetBytesAsync(_hashLength));
            return hashedPass;
        }
    }
}