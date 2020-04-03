using System;
using System.Threading.Tasks;
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
        
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this._userService = userService;
            this._logger = logger;
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
     }
}