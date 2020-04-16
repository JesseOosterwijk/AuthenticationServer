using System;
using System.Threading.Tasks;
using CyberAuthenticationAPI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

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
                return BadRequest("Doe eens ff normaal");
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
                return BadRequest("Fuck off");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Wat is AVG?");
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody]VerifyRequest token)
        {
            try
            {
                this._tokenService.VerifyToken(token.Token);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest("oprotten kutaziaat");
            }
        }
     }
}