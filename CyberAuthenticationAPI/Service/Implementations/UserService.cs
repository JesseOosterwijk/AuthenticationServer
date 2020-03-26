using System;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interfaces;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IEncryptionService encryptService;
        private readonly IUserRepository userRepo;
        private readonly ITokenService tokenService;
        
        public UserService(IEncryptionService encryptService, IUserRepository userRepo, ITokenService tokenService)
        {
            this.encryptService = encryptService;
            this.userRepo = userRepo;
        }

        public async void AddUser(string email, string password)
        {
            Task<string> hashedPasswordTask = encryptService.HashAsync(password);
            
            Guid userId = Guid.NewGuid();
            
            userRepo.InsertUser(userId.ToString(), email, await hashedPasswordTask);
        }

        public async void DeleteUser(string userId)
        {
            userRepo.DeleteUser(userId);
        }

        public async Task<string> Login(string email, string password)
        {
            Task<string> hashTask = encryptService.HashAsync(password);
            Task<UserDto> userTask = userRepo.GetUser(email, await hashTask);

            return tokenService.GenerateToken(await userTask);
        }
    }
}