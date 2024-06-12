using RedisApp.API.Models;

namespace RedisApp.API.Repository
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<Product> CreateAsync(Product product);
    }
}
