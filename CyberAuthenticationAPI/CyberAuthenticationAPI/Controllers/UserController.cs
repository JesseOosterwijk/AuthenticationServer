using System;
using System.Threading.Tasks;
using CyberAuthenticationAPI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Response;

namespace CyberAuthenticationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly ITokenService _tokenService;
        
        public UserController(ILogger<UserController> logger, IUserService userService, ITokenService tokenService)
        {
            this._userService = userService;
            this._logger = logger;
            this._tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            try
            {
                return Ok(await this._userService.Login(request.Email, request.Password));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in");
                return BadRequest("Error logging in");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await this._userService.AddUser(request.Email, request.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering");
                return BadRequest("Error registering");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _userService.DeleteUser("1", "VeiligWachtwoord");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest("Wat is AVG?");
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody]VerifyRequest token)
        {
            try
            {
                await this._tokenService.VerifyToken(token.Token);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest("Unable to verify token");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]TokenRequest refreshRequest)
        {
            try
            {                
                return Ok(await this._userService.Refresh(refreshRequest));
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest("Unable to refresh token");
            }
        }
     }
}