using MyStoreAPI;
using Microsoft.Extensions.Configuration;
//para OpenApiSecurityScheme, OpenApiSecurityRequirement, etc. (de swagger)
using Microsoft.OpenApi.Models; 
//para Encoding
using System.Text;
//para la variable de ambiente
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;




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


//Agregar seguridad y autentificaciones con "Manage JWTs in development" y "JSON Web Tokens "
// string API_value = Environment.GetEnvironmentVariable("DB");
// if (!String.IsNullOrEmpty(API_value) ){

    //Obtener la IP y el puerto de la variable de ambiente dada:
    //docker run -d --name api -e DB="server=192.168.18.8;user=root;password=123456;database=MyStoreApi" -e ASPNETCORE_ENVIRONMENT=Development -p 8080:8080 api
    // var enviromentVariableParts = enviromentVariableParts.Split(';')
    //     .Select(part => part.Split('='))
    //     .ToDictionary(split => split[0], split => split[1]);

    // string server = enviromentVariableParts["server"];
    // var serverParts = server.Split(':');
    // string ip = serverParts[0];
    
//}else{
    //tomamos el local host de appsettings.json
//}


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
            ValidIssuer = "https://localhost:7161",
            ValidAudience = "https://localhost:7161",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });

var app = builder.Build();


    //Configurar para usar appsettings.json t appsettings.{Environment}.json
    //Alternativa de "builder.Configuration.AddJsonFile("appsettings.json", optional: false,reloadOnChange: true);"
// builder.Configuration
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//     .AddEnvironmentVariables();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
 app.UseCors(); //builder.Services.AddCors() ya agrega CORS
app.Run();
