using core.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


///seguridad

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme  //schema usado
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //agrego n cantidad de autentificaciones
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, //quien crea la validacion
            ValidateAudience = true,   //hacia quien se dirige
            ValidateLifetime = true,   //el tiempo de vida que 
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:5001",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });


///seguridad


var app = builder.Build();

 if (app.Environment.IsDevelopment())
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDB").Value.ToString();

        string DB_value = Environment.GetEnvironmentVariable("DB");
        if (! String.IsNullOrEmpty(DB_value) )
        {
            Console.WriteLine("variable is not empty", connection);
            connection = DB_value;
        }
        Console.WriteLine("connection", connection);
        Storage.Init(connection) ;

        app.UseSwagger();
        app.UseSwaggerUI();

        StoreDb.CrearDatosSync();
    }

app.UseHttpsRedirection();

app.UseRouting();

// Use CORS
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();