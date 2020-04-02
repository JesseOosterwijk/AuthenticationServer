using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace CyberAuthenticationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            this._userService = userService;
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
                return BadRequest("Doe eens ff normaal");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                this._userService.AddUser(request.Email, request.Password);
                return Ok();
            }
            catch (Exception ex)
            {
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