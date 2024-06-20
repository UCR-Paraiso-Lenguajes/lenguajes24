using Core;
using StoreAPI.Database;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5207",
            ValidAudience = "http://localhost:5207",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = c =>
            {
                Console.WriteLine("Authentication failed:");
                Console.WriteLine(c);
                Console.WriteLine($"Exception: {c.Exception}");
                Console.WriteLine($"Exception: {c.HttpContext}");
                return Task.CompletedTask;
            },
            OnMessageReceived = msg =>
            {
                var token = msg?.Request.Headers.Authorization.ToString();
                string path = msg?.Request.Path ?? "";
                if (!string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Access token");
                    Console.WriteLine($"URL: {path}");
                    Console.WriteLine($"Token: {token}\r\n");
                }
                else
                {
                    Console.WriteLine("Access token");
                    Console.WriteLine("URL: " + path);
                    Console.WriteLine("Token: No access token provided\r\n");
                }

                // Imprimir más información de la solicitud
                Console.WriteLine("Headers:");
                foreach (var header in msg.Request.Headers)
                {
                    Console.WriteLine($"{header.Key}: {header.Value}");
                }
                Console.WriteLine($"Method: {msg.Request.Method}");
                Console.WriteLine($"Query String: {msg.Request.QueryString}");
                Console.WriteLine($"Content Length: {msg.Request.ContentLength}");
                Console.WriteLine($"Content Type: {msg.Request.ContentType}");
                Console.WriteLine($"Host: {msg.Request.Host}");

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
string connection = "";
var value = Environment.GetEnvironmentVariable("DB");
if (value == null)
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
}
else
{
    connection = value;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Storage.Init(connection);

app.UseRouting();
app.UseCors();
app.UseAuthentication(); // Asegúrate de agregar esta línea
app.UseAuthorization();

app.MapControllers();

app.Run();
