
using CompanyServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace CompanyServices;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://*:5056");
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "CompanyServices API",
                Version = "v1"
            });
        });
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });

        builder.Services.AddDbContext<CompanyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.EnableRetryOnFailure()));

        builder.Services.AddHealthChecks();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyServices API V1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.UseWebSockets();
        app.Map("/ws/companies/updates", async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                WebSocketHandler.AddSocket(webSocket);
                await WebSocketHandler.Receive(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        });

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CompanyContext>();
            context.Database.Migrate();

            if (!context.Companies.Any())
            {
                context.Companies.AddRange(
                    new Companies { name = "Mave", sector = "Software", city = "Istanbul", employeeCount = 1, lastUpdated = DateTime.UtcNow }
                );

                context.SaveChanges();
            }
        }

        app.MapHealthChecks("/health");

        app.Run();
    }
}
