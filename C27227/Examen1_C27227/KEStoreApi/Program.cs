using System;
using System.Text;
using System.Threading.Tasks;
using Core;
using KEStoreApi;
using KEStoreApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    policy =>
    {
        policy.WithOrigins("http://localhost:5072", "http://localhost:8080", "http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
    });
});

var security = Environment.GetEnvironmentVariable("Security");

if (string.IsNullOrEmpty(security) || security.ToLower() == "false")
{
    // Swagger configuration when security is false or not set
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
}
else
{
    // JWT Authentication configuration when security is set and not false
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:5072",
                ValidAudience = "http://localhost:5072",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    return Task.CompletedTask;
                }
            };
        });
}

var app = builder.Build();

string connection;
var dbConnection = Environment.GetEnvironmentVariable("DB");

if (string.IsNullOrEmpty(dbConnection))
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    connection = builder.Configuration.GetConnectionString("MyDatabase");
}
else
{
    connection = dbConnection;
}

DatabaseConfiguration.Init(connection);

// Verificar conexión a MySQL y crear la base de datos si no existe
try
{
    DatabaseStore.VerifyAndCreateDatabase(connection);
}
catch (Exception ex)
{
    throw new Exception("Error al conectar a MySQL o crear la base de datos.", ex);
}

if (app.Environment.IsDevelopment())
{
    try
    {
        DatabaseStore.Store_MySql();
    }
    catch (Exception ex)
    {
        throw new Exception("Error al inicializar la base de datos.", ex);
    }
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

if (!string.IsNullOrEmpty(security) && security.ToLower() != "false")
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapHub<MarketingHub>("/hubs/marketing");
app.MapControllers();
app.Run();
