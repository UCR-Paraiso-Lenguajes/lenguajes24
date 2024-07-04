using Store_API.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Store_API.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
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

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7165",
            ValidAudience = "https://localhost:7165",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });

    builder.Services.AddSignalR();
    builder.Services.AddSingleton<NotificationHub>();

// Configuración de appsettings.json y base de datos
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuración de la base de datos según el entorno
if (builder.Environment.IsDevelopment())
{
    string connectionString = builder.Configuration.GetSection("ConnectionStrings").GetSection("DataBase").Value.ToString();
    builder.Services.AddSingleton(new DB_API(connectionString));

    var dbApi = new DB_API();
    dbApi.ConnectDB();

}
else if (builder.Environment.IsProduction())
{
    string connectionString = builder.Configuration.GetSection("ConnectionStrings").GetSection("DataBase").Value.ToString();
    builder.Services.AddSingleton(new DB_API(connectionString));

    var dbApi = new DB_API();
    dbApi.ConnectDB();

}

// Construir la aplicación
var app = builder.Build();

// Middleware de HTTPS
app.UseHttpsRedirection();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Endpoints de controladores
app.MapControllers();

// Middleware de CORS
app.UseCors();

// Configuración de Swagger y ejecución de la aplicación
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();