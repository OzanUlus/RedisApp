using Microsoft.EntityFrameworkCore;
using RedisApp.API.Models;
using RedisApp.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => {

    opt.UseInMemoryDatabase(databaseName:"RedisApp");
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{
  var dbContex = app.Services.GetRequiredService<AppDbContext>();
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
