using Microsoft.EntityFrameworkCore;

namespace RedisApp.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Product>().HasData(
                new Product() { Id = 1 , Name= "Kalem", Price= 32},
                new Product() { Id = 2 , Name= "Silgi", Price= 16},
                new Product() { Id = 3 , Name= "Dergi", Price= 24});

            base.OnModelCreating(mb);
        }
    }
}
