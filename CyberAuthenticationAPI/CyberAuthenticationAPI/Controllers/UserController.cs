using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace CyberAuthenticationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult>Login()
        {
            return Ok("Heb nou eens effe geduld");
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            return Ok("Heb nou eens effe geduld");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            return Ok("Heb nou eens effe geduld");
        }
     }
}