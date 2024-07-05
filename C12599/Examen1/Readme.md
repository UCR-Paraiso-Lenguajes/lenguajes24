# Proyecto de Tienda en Línea

## Índice

1. [Introducción](#introducción)
2. [Arquitectura del Sistema](#arquitectura-del-sistema)
3. [Diagramas UML](#diagramas-uml)
    - [Diagrama de Actividad](#diagrama-de-actividad)
    - [Diagrama de Clase](#diagrama-de-clase)
    - [Diagrama de Paquetes](#diagrama-de-paquetes)
    - [Diagrama de Campaigns Investigación](#diagrama-de-campaigns-investigación)
    - [Diagrama de Actividad Carousel y PaymentMethods](#diagrama-de-actividad-carousel-y-paymentmethods)
4. [Configuración del Proyecto](#configuración-del-proyecto)
5. [Seguridad](#seguridad)
6. [Autenticación y Autorización](#autenticación-y-autorización)
7. [Componentes de la Aplicación](#componentes-de-la-aplicación)
8. [Conclusión](#conclusión)

## Introducción

Este proyecto de una tienda en línea se desarrolló durante el curso de Lenguajes de Programación. La tienda permite a los usuarios navegar productos, agregarlos al carrito, realizar compras y visualizar estadísticas de ventas. La implementación se realiza utilizando tecnologías modernas y buenas prácticas de desarrollo.

## Arquitectura del Sistema

El sistema está diseñado para ser modular y escalable, con una arquitectura de microservicios que incluye servicios para la gestión de productos, usuarios, autenticación y autorizaciones, y estadísticas de ventas. La comunicación entre servicios se realiza mediante APIs RESTful.

## Diagramas UML

### Diagrama de Actividad

![Diagrama de Actividad](actividad.png)

El diagrama de actividad muestra el flujo de trabajo dentro del sistema, desde la autenticación del usuario hasta la realización de una compra.

### Diagrama de Clase

![Diagrama de Clase](clase.png)

El diagrama de clase representa la estructura estática del sistema, mostrando las clases, atributos y métodos, así como sus relaciones.

### Diagrama de Paquetes

![Diagrama de Paquetes](paquetes.png)

El diagrama de paquetes muestra la organización modular del sistema, agrupando clases relacionadas en paquetes para mejorar la mantenibilidad y escalabilidad del código.

### Diagrama de Campaigns Investigación

![Diagrama de Campaigns Investigación](Investigacion.JPG)

Este diagrama ilustra la estructura y relaciones de las campañas de investigación dentro del sistema.

### Diagrama de Actividad Carousel y PaymentMethods

![Diagrama de Actividad Carousel y PaymentMethods](ProyectoActividad.jpg)

El diagrama muestra los flujos de actividad relacionados con el carrusel de productos y los métodos de pago disponibles.

### Diagrama UML de Proyecto

![Diagrama UML de Proyecto](ProyectoUML.jpg)

Este diagrama proporciona una vista general de la estructura y componentes del proyecto de tienda en línea.

## Configuración del Proyecto

Para configurar el proyecto, siga los pasos a continuación:

1. **Clonar el repositorio**:
    ```bash
    git clone https://github.com/usuario/tienda-en-linea.git
    cd tienda-en-linea
    ```

2. **Instalar dependencias**:
    ```bash
    dotnet restore
    ```

3. **Configurar la base de datos**:
    - Asegúrese de tener una base de datos configurada y actualice la cadena de conexión en `appsettings.json`.

4. **Ejecutar migraciones**:
    ```bash
    dotnet ef database update
    ```

5. **Iniciar la aplicación**:
    ```bash
    dotnet run
    ```

## Seguridad

La seguridad en este proyecto se implementa utilizando autenticación y autorización basada en JWT (JSON Web Tokens). A continuación se muestra un resumen de los pasos y el código necesario para configurar esta seguridad:

1. **Configuración de JWT en `Program.cs`**:
    ```csharp
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;

    var builder = WebApplication.CreateBuilder(args);

    // Configuración de servicios de autenticación JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:7043",
                ValidAudience = "http://localhost:7043",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
            };
        });

    // Configuración de Swagger para incluir autenticación JWT
    builder.Services.AddSwaggerGen(setup =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });
    ```

## Autenticación y Autorización

### Modelo de Usuario y Seeder:
Se crea un modelo de usuario con roles y un seeder para inicializar los usuarios.

```csharp
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace storeapi.Models
{
    public class UserAccount
    {
        public string UserName { get; }
        public string UserPassword { get; }
        public IEnumerable<Claim> UserRoles { get; }

        public static readonly List<UserAccount> allUsers = new List<UserAccount>();

        public static IEnumerable<UserAccount> AllUsersData => allUsers.AsReadOnly();

        public UserAccount(string userName, string userPassword, List<Claim> userRoles)
        {
            if (userRoles == null || userRoles.Count == 0)
                throw new ArgumentException("Debe crear roles para los usuarios");
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("No pueden existir usuarios nulos");
            if (string.IsNullOrEmpty(userPassword))
                throw new ArgumentException("Debe existir un password");

            UserName = userName;
            UserPassword = userPassword;
            UserRoles = new List<Claim>(userRoles);

            allUsers.Add(this);
        }
    }

    public static class UserAccountSeeder
    {
        public static void SeedUsers()
        {
            new UserAccount("mariano", "123456", new List<Claim>
            {
                new Claim(ClaimTypes.Name, "mariano"),
                new Claim(ClaimTypes.Role, "Admin")
            });

            new UserAccount("juan", "234567", new List<Claim>
            {
                new Claim(ClaimTypes.Name, "juan"),
                new Claim(ClaimTypes.Role, "user")
            });

            new UserAccount("sofia", "345678", new List<Claim>
            {
                new Claim(ClaimTypes.Name, "sofia"),
                new Claim(ClaimTypes.Role, "Admin")
            });
        }
    }
}

Controlador de Autenticación:
Se crea un controlador para manejar las solicitudes de inicio de sesión y generar tokens JWT.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            UserAccountSeeder.SeedUsers();
        }

        private bool IsDevelopmentEnvironment => hostEnvironment.EnvironmentName == "Development";

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null || string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.userPassword))
                return BadRequest("Invalid client request");

            if (IsDevelopmentEnvironment)
            {
                var existingUser = UserAccount.AllUsersData.FirstOrDefault(u => 
                    u.UserName == user.userName && u.UserPassword == user.userPassword);

                if (existingUser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, existingUser.UserName)
                    };
                    claims.AddRange(existingUser.UserRoles);

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:7043",
                        audience: "http://localhost:7043",
                        claims: claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

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



