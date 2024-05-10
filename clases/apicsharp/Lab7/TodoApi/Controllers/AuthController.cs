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
using TodoApi.business;
using TodoApi.models;

namespace TodoApi;

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
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
    {
        if (user is null)
        {
            return BadRequest("Invalid client request");
        }

        
        if (hostEnvironment.IsDevelopment())
        {
            if (user.UserName == "cris" && user.Password == "123456")
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "cris"),
                            new Claim(ClaimTypes.Role, "Operator"),
                            new Claim(ClaimTypes.Role, "Admin"),
                        };
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokeOptions = new JwtSecurityToken(
                            issuer: "https://localhost:5001",
                            audience: "https://localhost:5001",
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(5),
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
}

public class AuthenticatedResponse
{
    public string? Token { get; set; }
}