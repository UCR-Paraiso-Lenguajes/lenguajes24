using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Core.Models.Store_API.Models;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IConfiguration configuration;

        public UserAuthController(IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
            UserAuthCredentials.UsersCredentials();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
        {
            if (user == null)
                return BadRequest("Invalid client request");
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                return BadRequest("Invalid client request");

            if (hostEnvironment.IsDevelopment())
            {
                var existingUser = UserAuth.AllUsersData.FirstOrDefault(u =>
                    u.UserName == user.UserName && u.Password == user.Password);

                if (existingUser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, existingUser.UserName)
                    };
                    claims.AddRange(existingUser.Roles);

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:7165",
                        audience: "https://localhost:7165",
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(1),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new AuthenticatedResponse { Token = tokenString });
                }
            }

            return Unauthorized();
        }

        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class AuthenticatedResponse
        {
            public string Token { get; set; }
        }
    }
}