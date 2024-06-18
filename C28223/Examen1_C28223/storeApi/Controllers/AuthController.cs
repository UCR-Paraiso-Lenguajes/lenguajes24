using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using storeApi.Models;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IHostEnvironment hostEnvironment;
        public authController(IHostEnvironment hostEnvironment) { this.hostEnvironment = hostEnvironment; createMockUsers(); }
        private void createMockUsers()
        {
            if (!LoginDataAccount.listUsersData.Any())
            {
                new LoginDataAccount("bulan", "123456", new List<Claim> { new Claim(ClaimTypes.Name, "brandon"), new Claim(ClaimTypes.Role, "Admin") });
                new LoginDataAccount("bulan2", "666666", new List<Claim> { new Claim(ClaimTypes.Name, "brandon2"), new Claim(ClaimTypes.Role, "User") });
                new LoginDataAccount("bulan3", "777777", new List<Claim> { new Claim(ClaimTypes.Name, "brandon3"), new Claim(ClaimTypes.Role, "Admin") });
                new LoginDataAccount("bulan4", "101010", new List<Claim> { new Claim(ClaimTypes.Name, "brandon4"), new Claim(ClaimTypes.Role, "Admin") });
                new LoginDataAccount("bulan5", "222222", new List<Claim> { new Claim(ClaimTypes.Name, "brandon5"), new Claim(ClaimTypes.Role, "Admin") });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginMod dataUserLog)
        {
            if (dataUserLog is null) return BadRequest("Data information Login is not present");
            var userLog = dataUserLog.userLog;
            var userPass = dataUserLog.passwordLog;
            if (userPass is null || string.IsNullOrEmpty(userPass)) return BadRequest($"Data information Login {userPass}  cannot be null or empty.");
            if (userLog is null || string.IsNullOrEmpty(userLog)) return BadRequest($"Data information Login {userLog} cannot be null or empty.");
            if (hostEnvironment.IsDevelopment())
            {
                var userValid = false;
                var claimsUser = new List<Claim>();
                foreach (var thisUser in LoginDataAccount.listUsersData)
                {
                    if (dataUserLog.userLog == thisUser.user && dataUserLog.passwordLog == thisUser.userPass)
                    {
                        claimsUser.AddRange(thisUser.listClaims);
                        userValid = true;
                        break;
                    }
                }
                if (userValid)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, dataUserLog.userLog) };
                    claims.AddRange(claimsUser);
                    var secretsKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signingCredentials = new SigningCredentials(secretsKey, SecurityAlgorithms.HmacSha256);
                    var tokenOp = new JwtSecurityToken(
                        issuer: "http://localhost:7223",
                        audience: "http://localhost:7223",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(1),
                        signingCredentials: signingCredentials
                    );
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOp);
                    if (tokenOp.ValidTo < DateTime.UtcNow) { return Unauthorized("El token expiró"); }
                    return Ok(new ResponseAuthen { Token = token });
                }
            }
            return Unauthorized();
        }
    }
}

public class ResponseAuthen
{
    public string Token {get; set; }
}

public class LoginMod
{
    public string userLog { get; private set; }
    public string passwordLog { get; private set; }
    public LoginMod(string userLog, string passwordLog)
    {
        if (string.IsNullOrEmpty(userLog)) throw new ArgumentException($"{nameof(userLog)}cannot be null or empty");
        if (string.IsNullOrEmpty(passwordLog)) throw new ArgumentException($" {nameof(passwordLog)} cannot be null or empty");

        this.userLog = userLog;
        this.passwordLog = passwordLog;
    }
}