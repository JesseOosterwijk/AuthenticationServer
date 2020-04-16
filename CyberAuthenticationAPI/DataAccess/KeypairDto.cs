namespace DataAccess
{
    public class KeypairDto
    {
        public string UserId { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] PrivateKey { get; set; }
    }
}
