using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class TestUser
        {
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public IEnumerable<string> UserRoles { get; set; }
        }

        private List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    UserName = "jaziel",
                    UserPassword = "12345",
                    UserRoles = new List<string> { "Admin", "User" }
                },
            };
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "El usuario no se encuentra definido");
            }

            var testUsers = GetTestUsers();
            var secretKey = _configuration.GetValue<string>("Jwt:SecretKey");

            foreach (var testUser in testUsers)
            {
                if (user.UserName == testUser.UserName && user.Password == testUser.UserPassword)
                {
                    var claims = testUser.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
                    claims.Add(new Claim(ClaimTypes.Name, testUser.UserName));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:5001",
                        audience: "https://localhost:5001",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(2),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new AuthenticatedResponse { Token = tokenString });
                }
            }

            return Unauthorized();
        }
    }

    public class LoginModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public LoginModel(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }
    }

    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
    }
}