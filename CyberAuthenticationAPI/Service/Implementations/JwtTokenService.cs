using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using DataAccess;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Service.Implementations
{
    public class JwtTokenService : ITokenService
    {
        private IEncryptionService _encryptionService;
        private SecurityKey _securityKey;

        public JwtTokenService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _securityKey = new AsymmetricSignatureProvider(
                       new RsaSecurityKey(RSA.Create(2048)), SecurityAlgorithms.RsaSha512).Key;
        }
        
        
        public string GenerateToken(UserDto user)
        {
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
                
                SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.RsaSha512),
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public bool VerifyToken(string token)   //TODO: Validate Audience and Validate issuer of token
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "CyberB",
                ValidAudience = "User",
                IssuerSigningKey = _securityKey
            };

            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

    }
}