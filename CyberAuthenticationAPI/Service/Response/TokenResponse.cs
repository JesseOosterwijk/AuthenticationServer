using System;

namespace CyberAuthenticationAPI.Response
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpirationDate { get; set; }
        public string RefreshToken { get; set; }
    }
}
