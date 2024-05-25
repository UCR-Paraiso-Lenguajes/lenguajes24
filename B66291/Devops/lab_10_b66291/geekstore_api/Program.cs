using core.DataBase;

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