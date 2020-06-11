using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using System.Threading.Tasks;
using CyberAuthenticationAPI.Response;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using Service.Response;

namespace Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHashService _hashService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ISaltRepository _saltRepo;
        private readonly IKeypairRepository _keypairRepo;
        private readonly IStringEncryptionService _stringEncryptionService;

        public UserService(IHashService hashService, IEncryptionService encryptionService, IUserRepository userRepo, ITokenService tokenService, ISaltRepository saltRepo, IKeypairRepository keypairRepo, IStringEncryptionService stringEncryptionService)
        {
            this._hashService = hashService;
            this._encryptionService = encryptionService;
            this._userRepo = userRepo;
            this._tokenService = tokenService;
            this._saltRepo = saltRepo;
            this._keypairRepo = keypairRepo;
            this._stringEncryptionService = stringEncryptionService;
        }

        public async Task AddUser(string email, string password)
        {
            Task<string> hashedPasswordTask = _hashService.HashAsync(password, out byte[] salt);

            string userId = Guid.NewGuid().ToString();

            email = _stringEncryptionService.EncryptString(email, "ajeofijqeia");

            await _userRepo.InsertUser(userId, email, await hashedPasswordTask);
            await _saltRepo.InsertSalt(userId, salt);
            KeyValuePair<string, string> keyPair = _encryptionService.GenerateKeyXml();
            await _keypairRepo.InsertKeypair(keyPair, userId);
        }

        public async Task DeleteUser(string userId, string password)
        {

            UserDto user = await _userRepo.GetUserById(userId);
            byte[] salt = await _saltRepo.GetSalt(userId);
            if (user.Password.SequenceEqual(await _hashService.HashAsync(password, salt)))
            {
                await _userRepo.DeleteUser(userId);
                await _saltRepo.DeleteSalt(userId);
                await _keypairRepo.DeleteKeypair(userId);
            }
            else
            {
                throw new AuthenticationException("Incorrect password");
            }
        }

        public async Task<TokenResponse> Login(string email, string password)
        {
            email = _stringEncryptionService.EncryptString(email, "ajeofijqeia");
            UserDto user = await _userRepo.GetUser(email);
            byte[] salt = await _saltRepo.GetSalt(user.Id);
            Task<string> hashTask = _hashService.HashAsync(password, salt);
            KeypairDto keyPair = await _keypairRepo.GetKeypair(user.Id);
            string privKey = keyPair.PrivateKey;

            if ((await hashTask).SequenceEqual((user.Password)))
            {
                (string token, DateTime expiration) = await _tokenService.GenerateToken(user, privKey);
                string refreshToken = _tokenService.GenerateRefreshString();
                await _userRepo.DeleteRefreshToken(user.Id);
                await _userRepo.InsertRefreshToken(user.Id, refreshToken);
                return new TokenResponse()
                {
                    Token = token,
                    TokenExpirationDate = expiration,
                    RefreshToken = refreshToken
                };
            }

            throw new AuthenticationException();
        }

        public async Task<TokenResponse> Refresh(TokenRequest response)
        {
            string userId = await GetUserIdFromAccessToken(response.Token);
            KeypairDto keyPair = await _keypairRepo.GetKeypair(userId);
            string privKey = keyPair.PrivateKey;
            UserDto user = await _userRepo.GetUserById(userId);
            string refreshToken = await _userRepo.GetRefreshToken(userId);
            if (refreshToken == response.RefreshToken)
            {
                (string token, DateTime expiration) = await _tokenService.GenerateToken(user, privKey);
                string newRefreshToken = _tokenService.GenerateRefreshString();
                await _userRepo.UpdateRefreshToken(userId, newRefreshToken);
                return new TokenResponse()
                {
                    Token = token,
                    TokenExpirationDate = expiration,
                    RefreshToken = newRefreshToken
                };
            }
            throw new AuthenticationException("All your base are belong to us");
        }

        public async Task<string> GetUserIdFromAccessToken(string accessToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken secToken = tokenHandler.ReadJwtToken(accessToken);
            string userId = secToken.Claims.Single(x => x.Type == "Id").Value;
            string _pubKey = (await _keypairRepo.GetKeypair(userId)).PublicKey;
            var key = _tokenService.BuildRsaSigningKey(_pubKey);
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "CyberB",
                ValidAudience = "User",
                IssuerSigningKey = key
            };
            IPrincipal principal = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);
            return userId;
        }

        public async Task<UserDto> GetUserById(string userId)
        {
            UserDto user = await _userRepo.GetUserById(userId);
            user.Email = _stringEncryptionService.DecryptString(user.Email, "ajeofijqeia");
            return user;
        }
    }
}