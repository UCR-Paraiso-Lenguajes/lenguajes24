using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using MySqlConnector;
using core;





public class Program
{


    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        var app = builder.Build();

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        string connection = builder.Configuration.GetSection("MyDbConnection").GetSection("Database").Value.ToString();
        string connectionStringMyDb = builder.Configuration.GetSection("MyDBConnection").GetSection("lab").Value.ToString();
        DataConnection.Init(connection, connectionStringMyDb);

        Configure(app, app.Environment);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "storeapi", Version = "v1" });
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    private static void Configure(WebApplication app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
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
    }



}
