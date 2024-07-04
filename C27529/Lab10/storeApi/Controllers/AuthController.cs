using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace storeApi.Controllers
{

    public class LoginDataAccount
    {
        public static List<LoginDataAccount> listUsersData = new List<LoginDataAccount>();

        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Claim> Claims { get; set; }

        public LoginDataAccount(string userName, string password, List<Claim> claims)
        {
            
            UserName = userName;
            Password = password;
            Claims = claims;
            listUsersData.Add(this);
        }
    }

    public class LoginModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHostEnvironment hostEnvironment;

        public AuthController(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;

            if (!LoginDataAccount.listUsersData.Any())
            {
                new LoginDataAccount("usu", "1234", new List<Claim> { new Claim(ClaimTypes.Name, "usu"), new Claim(ClaimTypes.Role, "Admin") });
                new LoginDataAccount("usu1", "1234", new List<Claim> { new Claim(ClaimTypes.Name, "usu1"), new Claim(ClaimTypes.Role, "User") });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))  
    {
                return BadRequest("Invalid client request");
            }

            var foundUser = LoginDataAccount.listUsersData.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (foundUser == null)
            {
                return Unauthorized(new { message = "User not found or password incorrect" });
            }

            var claims = new List<Claim>(foundUser.Claims);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5164",
                audience: "http://localhost:5164",
                claims: claims,
                expires: DateTime.Now.AddMinutes(2), 
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthenticatedResponse { Token = tokenString });
        }
    }


}
