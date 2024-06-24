using MyStoreAPI;
using Core;
using Microsoft.Extensions.Configuration;
//para OpenApiSecurityScheme, OpenApiSecurityRequirement, etc. (de swagger)
using Microsoft.OpenApi.Models; 
//para Encoding
using System.Text;
//para la variable de ambiente
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//WebSockets
using Microsoft.AspNetCore.SignalR;
using MyStoreAPI.Controllers;//indicar donde esta el/los hub creados

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Plugin de Swagger para usarlo con Tokens:
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

// Add SignalR service
builder.Services.AddSignalR();

// Inyectamos NotificationLogic para el NotificacionController para no tener problemas con
//los test
//builder.Services.AddScoped<NotificationLogic>();


//Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


//Variables de Ambiente para JWT
var jwtGlobal = Environment.GetEnvironmentVariable("JWT_GLOBAL");
if(string.IsNullOrEmpty(jwtGlobal)){
    jwtGlobal = "https://localhost:7161";
}

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
    {
        //Creamos el token y se le indica que parámetros queremos crearle (todos se deben validar después)
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtGlobal,
            ValidAudience = jwtGlobal,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });

var app = builder.Build();

    
// Configurar para usar appsettings.json y appsettings.{Environment}.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false,reloadOnChange: true);
if (app.Environment.IsDevelopment()){    
        
    string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();

    string DB_value = Environment.GetEnvironmentVariable("DB");
    if (!String.IsNullOrEmpty(DB_value) ){
        Console.WriteLine("varible is not empty", connection);
        connection = DB_value;
    }

    Console.WriteLine("connection", connection);

    DB_Connection.SET_CONFIG_DB(connection);
        
    app.UseSwagger();
    app.UseSwaggerUI();

    DB_Connection.ConnectDB();    

}else if (app.Environment.IsProduction()){    

    string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value;
    Console.WriteLine($"Production Connection: {connection}");
    DB_Connection.SET_CONFIG_DB(connection);
    DB_Connection.ConnectDB();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors();

//Para el token
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
//app.UseCors(); //builder.Services.AddCors() ya agrega CORS

//Map SignalR hubs
app.MapHub<NotificationHub>("/notificationHub");
    //nombre del dominio<-/hub 
app.Run();
