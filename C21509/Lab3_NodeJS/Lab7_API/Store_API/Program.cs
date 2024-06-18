using Store_API.Database;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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


// Configure the HTTP request pipeline.
builder.Configuration.AddJsonFile("appsettings.json", optional: false,reloadOnChange: true);
if (app.Environment.IsDevelopment())
{
    string connectionString = builder.Configuration.GetSection("ConnectionStrings").GetSection("DataBase").Value.ToString();
    DB_API dbApi= new DB_API(connectionString);
    string DB_value = Environment.GetEnvironmentVariable("DB");
    if (!String.IsNullOrEmpty(DB_value))
    {
        connectionString = DB_value;
    }

    dbApi.ConnectDB(connectionString);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
