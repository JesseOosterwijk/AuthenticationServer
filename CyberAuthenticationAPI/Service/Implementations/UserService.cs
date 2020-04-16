using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interfaces;
using Service.Interfaces;

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
        
        public UserService(IHashService hashService, IEncryptionService encryptionService, IUserRepository userRepo, ITokenService tokenService, ISaltRepository saltRepo, IKeypairRepository keypairRepo)
        {
            this._hashService = hashService;
            this._encryptionService = encryptionService;
            this._userRepo = userRepo;
            this._tokenService = tokenService;
            this._saltRepo = saltRepo;
            this._keypairRepo = keypairRepo;
        }

        public async Task AddUser(string email, string password)
        {
            Task<string> hashedPasswordTask = _hashService.HashAsync(password, out byte[] salt);
            
            string userId = Guid.NewGuid().ToString();
            
            await _userRepo.InsertUser(userId, email, await hashedPasswordTask);
            await _saltRepo.InsertSalt(userId, salt);
            KeyValuePair<string, string> keyPair = _encryptionService.GenerateKeyXml();
            await _keypairRepo.InsertKeypair(keyPair, userId);
        }

        public async Task DeleteUser(string userId, string password)
        {

            UserDto user = await _userRepo.GetUser(userId);
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

        public async Task<string> Login(string email, string password)
        {
            UserDto user = await _userRepo.GetUser(email);
            byte[] salt = await _saltRepo.GetSalt(user.Id);
            Task<string> hashTask = _hashService.HashAsync(password, salt);
            KeypairDto keyPair = await _keypairRepo.GetKeypair(user.Id);
            string privKey = keyPair.PrivateKey;

            if ((await hashTask).SequenceEqual((user.Password)))
            {
                return await _tokenService.GenerateToken(user, privKey);
            }
           
            throw new AuthenticationException();
        }
    }
}