using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Service.Implementations
{
    public class JwtTokenService : ITokenService
    {
        private IEncryptionService _encryptionService;
        private IKeypairRepository _keyPairRepo;

        public JwtTokenService(IEncryptionService encryptionService, IKeypairRepository keyPairRepo)
        {
            _encryptionService = encryptionService;
            _keyPairRepo = keyPairRepo;
        }
        
        
        public async Task<string> GenerateToken(UserDto user, string _privKey)
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
            return await Task.Run(() => tokenHandler.WriteToken(securityToken));
        }

        public async Task<bool> VerifyToken(string token)   //TODO: Validate Audience and Validate issuer of token
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken secToken = tokenHandler.ReadJwtToken(token);
            string userId = secToken.Claims.Single(x => x.Type == "Id").Value;
            string _pubKey = (await _keyPairRepo.GetKeypair(userId)).PublicKey;
            var key = BuildRsaSigningKey(_pubKey);
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
        
        public RsaSecurityKey BuildRsaSigningKey(string xmlParams){
            var rsaProvider = new RSACryptoServiceProvider(2048);
            var parameters = _encryptionService.ParseXmlString(xmlParams);
            rsaProvider.ImportParameters(parameters);
            var key = new RsaSecurityKey(rsaProvider);   
            return key;
        }

    }
}