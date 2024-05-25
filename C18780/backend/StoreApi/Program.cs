using MediatR;
using StoreApi;
using StoreApi.Commands;
using StoreApi.Data;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;
using System.Collections;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ISalesLineRepository, SalesLineRepository>();
builder.Services.AddScoped<ISinpeRepository, SinpeRepository>();
builder.Services.AddScoped<IDailySalesRepository, DailySalesRepository>();
builder.Services.AddScoped<IWeeklySalesRepository, WeeklySalesRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Registra los manejadores de MediatR específicos
builder.Services.AddTransient<IRequestHandler<GetProductListQuery, List<Product>>, GetProductListHandler>();
builder.Services.AddTransient<IRequestHandler<GetProductByIdQuery, Product>, GetProductByIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetProductByCategoryQuery, List<Product>>, GetProductByCategoryHandler>();
builder.Services.AddTransient<IRequestHandler<CreateProductCommand, Product>, CreateProductHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteProductCommand, int>, DeleteProductHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateProductCommand, int>, UpdateProductHandler>();

builder.Services.AddTransient<IRequestHandler<CreateSalesCommand, Sales>, CreateSalesHandler>();
builder.Services.AddTransient<IRequestHandler<GetSalesByIdQuery, Sales>, GetSalesByIdHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateSalesCommand, int>, UpdateSalesHandler>();
builder.Services.AddTransient<IRequestHandler<GetSalesByPurchaseNumberQuery, Sales>, GetSalesByPurchaseNumberHandler>();

builder.Services.AddTransient<IRequestHandler<CreateSalesLineCommand, SalesLine>, CreateSalesLineHandler>();

builder.Services.AddTransient<IRequestHandler<CreateSinpeCommand, Sinpe>, CreateSinpeHandler>();

builder.Services.AddTransient<IRequestHandler<GetDailySalesQuery, IEnumerable<DailySales>>, GetDailySalesByDateHandler>();

builder.Services.AddTransient<IRequestHandler<GetWeeklySalesByDateQuery, IEnumerable<WeeklySales>>, GetWeeklySalesByDateHandler>();

builder.Services.AddTransient<IRequestHandler<GetCategoryByIdQuery, Category>, GetCategoryByIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetCategoryByNameQuery, Category>, GetCategoryByNameHandler>();
builder.Services.AddTransient<IRequestHandler<GetCategoryListQuery, IEnumerable<Category>>, GetCategoryListHandler>();
builder.Services.AddTransient<IRequestHandler<CreateCategoryCommand, Category>, CreateCategoryHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteCategoryCommand, int>, DeleteCategoryHandler>();


//Add services to controllers
builder.Services.AddTransient<CategoryController>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var value = Environment.GetEnvironmentVariable("DB");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("corsapp");

app.Run();
