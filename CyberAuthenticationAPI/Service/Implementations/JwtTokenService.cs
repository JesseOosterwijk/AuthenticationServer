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
        private RSAParameters _privKey;
        private RSAParameters _pubKey;

        public JwtTokenService(IEncryptionService encryptionService)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048);
            _privKey = provider.ExportParameters(true);
            _pubKey = provider.ExportParameters(false);
            _encryptionService = encryptionService;
           // _securityKey = new AsymmetricSignatureProvider(
           //            new RsaSecurityKey(RSA.Create(2048)), SecurityAlgorithms.RsaSha512).Key;
        }
        
        
        public string GenerateToken(UserDto user)
        {
            SecurityKey key =  BuildRsaSigningKey(_privKey);
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
                
                SigningCredentials =new SigningCredentials(key, SecurityAlgorithms.RsaSha512Signature, SecurityAlgorithms.Sha512Digest),
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public bool VerifyToken(string token)   //TODO: Validate Audience and Validate issuer of token
        {
            var key = BuildRsaSigningKey(_pubKey);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "CyberB",
                ValidAudience = "User",
                IssuerSigningKey = key
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
        
        public static RsaSecurityKey BuildRsaSigningKey(RSAParameters parameters){
            var rsaProvider = new RSACryptoServiceProvider(2048);
            rsaProvider.ImportParameters(parameters);
            var key = new RsaSecurityKey(rsaProvider);   
            return key;
        } 

    }
}