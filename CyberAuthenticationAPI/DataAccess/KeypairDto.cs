namespace DataAccess
{
    public class KeypairDto
    {
        public string UserId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public KeypairDto(string userId, string publicKey, string privateKey)
        {
            UserId = userId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
