using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace Service
{
    //https://github.com/kmaragon/Konscious.Security.Cryptography
    public class Argon2Service : IEncryptionService
    {
        private readonly int hashIterations;
        private readonly int parallelism;
        private readonly int saltLength;
        private readonly int memorySize;
        private byte[]? pepper;
        private int hashLength;
        
        public Argon2Service(int hashIterations, int parallelism, int memorySze, int saltLength = 32, byte[]? pepper = null, int length = 128)
        {
            this.hashIterations = hashIterations;
            this.parallelism = parallelism;
            this.saltLength = saltLength;
            this.memorySize = memorySze;
            this.pepper = pepper;
            this.hashLength = length;
            
        }
        
        public async Task<string> HashAsync(string str)
        {
            Argon2i argon2 = new Argon2i(Encoding.UTF8.GetBytes(str));
            byte[] salt = new byte[saltLength];

            using RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            random.GetNonZeroBytes(salt);

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = parallelism;
            argon2.Iterations = hashIterations;
            argon2.MemorySize = memorySize;

            if (pepper != null)
            {
                argon2.AssociatedData = pepper;
            }
            
            return Encoding.UTF8.GetString(await argon2.GetBytesAsync(hashLength));
        }
    }
}