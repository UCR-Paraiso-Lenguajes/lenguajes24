using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using so.Models;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public AuthController(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
                return BadRequest("Invalid client request");
            if (string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.userPassword))
                return BadRequest("Invalid client request");

            if (hostEnvironment.IsDevelopment())
            {
                // Verificar las credenciales del usuario "mariano"
                if (user.userName == "mariano" && user.userPassword == "123456")
                {
                    // Crear las reclamaciones del usuario
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "mariano"),
                        new Claim(ClaimTypes.Role, "Operator"),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    // Generar la clave secreta y las credenciales de inicio de sesión
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    // Configurar las opciones del token JWT
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:7043",
                        audience: "https://localhost:7043",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredentials
                    );

                    // Generar el token JWT
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    // Devolver el token como respuesta
                    return Ok(new AuthenticatedResponse { Token = tokenString });
                }
            }

            return Unauthorized();
        }
    }

    public class AuthenticatedResponse
    {
        public string Token { get; set; }
    }
}
