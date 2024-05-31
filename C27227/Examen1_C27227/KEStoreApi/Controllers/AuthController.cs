using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthLogic _authLogic;

        public AuthController(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _authLogic = new AuthLogic(hostEnvironment, configuration);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogInAsync([FromBody] UserAuth user)
        {
            if (!_authLogic.ValidateUser(user, out var claims))
            {
                return Unauthorized();
            }

            var tokenString = _authLogic.CreateToken(claims);
            return Ok(new { token = tokenString });
        }
    }
}
