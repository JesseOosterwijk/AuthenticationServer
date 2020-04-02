using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DataAccess;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Service.Implementations
{
    public class JwtTokenService : ITokenService
    {
        private readonly string secret;
        public JwtTokenService(string secret)
        {
            this.secret = secret;
        }
        
        
        public string GenerateToken(UserDto user)
        {
            byte[] symmetricKey = Convert.FromBase64String(secret);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            DateTime now = DateTime.UtcNow;
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Email", user.Email),
                    new Claim("Id", user.Id)
                }),

                Expires = now.AddMinutes(2),
                
                SigningCredentials = new SigningCredentials(
                    new AsymmetricSignatureProvider(
                        new RsaSecurityKey(RSA.Create(2048)), SecurityAlgorithms.RsaSha512).Key, SecurityAlgorithms.RsaSha512),
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public string VerifyToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}