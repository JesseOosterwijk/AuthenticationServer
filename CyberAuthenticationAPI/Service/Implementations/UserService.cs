using System;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interfaces;
using Service.Interfaces;

namespace Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHashService _encryptService;
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ISaltRepository _saltRepo;
        
        public UserService(IHashService encryptService, IUserRepository userRepo, ITokenService tokenService, ISaltRepository saltRepo)
        {
            this._encryptService = encryptService;
            this._userRepo = userRepo;
            this._tokenService = tokenService;
            this._saltRepo = saltRepo;
        }

        public async Task AddUser(string email, string password)
        {
            Task<string> hashedPasswordTask = _encryptService.HashAsync(password, out byte[] salt);
            
            string userId = Guid.NewGuid().ToString();
            
            await _userRepo.InsertUser(userId, email, await hashedPasswordTask);
            await _saltRepo.InsertSalt(userId, salt);
        }

        public async void DeleteUser(string userId, string password)
        {

            UserDto user = await _userRepo.GetUser(userId);
            byte[] salt = await _saltRepo.GetSalt(userId);
            if (user.Password.SequenceEqual(await _encryptService.HashAsync(password, salt)))
            {
                await _userRepo.DeleteUser(userId);
                await _saltRepo.DeleteSalt(userId);
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
            Task<string> hashTask = _encryptService.HashAsync(password, salt);

            if ((await hashTask).SequenceEqual((user.Password)))
            {
                return _tokenService.GenerateToken(user);
            }
           
            throw new AuthenticationException();
        }
    }
}