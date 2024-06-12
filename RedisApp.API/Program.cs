using Microsoft.EntityFrameworkCore;
using RedisApp.API.Models;
using RedisApp.API.Repository;
using RedisApp.Cache;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {

            opt.UseInMemoryDatabase(databaseName: "RedisApp");
        });
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddSingleton<RedisService>(sp => {
            return new RedisService(builder.Configuration["CacheOptions:Url"]);
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContex = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContex.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}