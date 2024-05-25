using Core;
using TodoApi.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("TodoApi/appsettings.json", optional: true, reloadOnChange: true);
    string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();

    string DB_value = Environment.GetEnvironmentVariable("DB");
    if (!String.IsNullOrEmpty(DB_value))
    {  
         connection = DB_value;
         Console.WriteLine("varible is not empty1"+ DB_value);
         Console.WriteLine("varible is not empty1"+ connection);
    }

    Console.WriteLine("connection"+ connection);
    Storage.Init(connection);
    StoreDB.CreateMysql();

    app.UseSwagger();
    app.UseSwaggerUI();



}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();