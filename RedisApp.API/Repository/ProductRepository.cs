using Microsoft.EntityFrameworkCore;
using RedisApp.API.Models;

namespace RedisApp.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
           var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetAsync(int id)
        {
           var product = await _context.Products.FindAsync(id);
            return product;
        }
    }
}
