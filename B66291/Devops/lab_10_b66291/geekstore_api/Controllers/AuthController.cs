using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace geekstore_api.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment hostEnvironment;

        public AuthController(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public class TestUser
        {
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public IEnumerable<string> UserRoles { get; set; }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "El usuario no se encuentra definido");
            }

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

            var testUsers = configuration.GetSection("TestUsers").Get<List<TestUser>>();

            foreach (var testUser in testUsers)
            {
                if (user.UserName == testUser.UserName && user.Password == testUser.UserPassword)
                {
                    var claims = testUser.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
                    claims.Add(new Claim(ClaimTypes.Name, testUser.UserName));

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "https://localhost:5001",
                        audience: "https://localhost:5001",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(1),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

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