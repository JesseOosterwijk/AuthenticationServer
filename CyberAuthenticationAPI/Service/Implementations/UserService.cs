using System;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interfaces;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IEncryptionService _encryptService;
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        
        public UserService(IEncryptionService encryptService, IUserRepository userRepo, ITokenService tokenService)
        {
            this._encryptService = encryptService;
            this._userRepo = userRepo;
            this._tokenService = tokenService;
        }

        public async void AddUser(string email, string password)
        {
            Task<string> hashedPasswordTask = _encryptService.HashAsync(password);
            
            Guid userId = Guid.NewGuid();
            
            _userRepo.InsertUser(userId.ToString(), email, await hashedPasswordTask);
        }

        public async void DeleteUser(string userId)
        {
            _userRepo.DeleteUser(userId);
        }

        public async Task<string> Login(string email, string password)
        {
            Task<string> hashTask = _encryptService.HashAsync(password);
            Task<UserDto> userTask = _userRepo.GetUser(email, await hashTask);

            return _tokenService.GenerateToken(await userTask);
        }
    }
}