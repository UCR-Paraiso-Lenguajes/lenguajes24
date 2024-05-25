using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiLab7.Controllers{

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IHostEnvironment hostEnvironment;

    public AuthController(IHostEnvironment hostEnvironment)
    {
        this.hostEnvironment = hostEnvironment;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] UserAuth user)
    {
        UserAuth.IsPresent(user);
        if (hostEnvironment.IsDevelopment())
        {
            if (user.Name == "Diego" && user.Password == "secret")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var tokenString = CreateToken(claims);

                return Ok(new { token = tokenString });
            }
            if (user.Name == "Eisenheim" && user.Password == "secret")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "Operator")
                };

                var tokenString = CreateToken(claims);

                return Ok(new { token = tokenString });
            }
        }

        return Unauthorized();
    }

    private String CreateToken(List<Claim> claims)
    {
        bool listIsNullOrEmpty = claims == null || claims.Count() < 0;
        if(!listIsNullOrEmpty){
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
        else throw new ArgumentNullException("The list of claims should not be null and must contain at least one claim");
    }
}

}