using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using storeapi; // Importa el namespace donde se encuentra StoreDB

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "storeapi", Version = "v1" });
});

// Configurar CORS
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

// Configurar pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    PaymentDB.CreateMysql();
    StoreDB.CreateMysql();
    CartSave.EnsureComprasTableExists();
    CartSave.EnsureItemsTableExists();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "storeapi v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(); // Habilitar CORS
app.UseAuthorization();

app.MapControllers();

app.Run();

