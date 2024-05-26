using MyStoreAPI;
//para la variable de ambiente
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
