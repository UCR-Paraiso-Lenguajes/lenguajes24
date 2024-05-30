using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly CredentialOptions credentials;

        public AuthController(
            IHostEnvironment hostEnvironment,
            IOptions<CredentialOptions> credentials
        )
        {
            this.hostEnvironment = hostEnvironment;
            this.credentials = credentials.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserAuth user)
        {
            UserAuth.IsPresent(user);
            if (hostEnvironment.IsDevelopment())
            {
                var validUser = credentials.Development.FirstOrDefault(cred =>
                    cred.Name == user.Name && cred.Password == user.Password
                );

                if (validUser == null)
                    return Unauthorized();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, validUser.Role)
                };

                var tokenString = CreateToken(claims);

                return Ok(new { token = tokenString });
            }
            return Unauthorized();
        }

        private String CreateToken(List<Claim> claims)
        {
            bool listIsNullOrEmpty = claims == null || claims.Count() < 0;
            if (!listIsNullOrEmpty)
            {
                var secretKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        "TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"
                    )
                );
                var signinCredentials = new SigningCredentials(
                    secretKey,
                    SecurityAlgorithms.HmacSha256
                );
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            }
            else
                throw new ArgumentNullException(
                    "The list of claims should not be null and must contain at least one claim"
                );
        }
    }
}
