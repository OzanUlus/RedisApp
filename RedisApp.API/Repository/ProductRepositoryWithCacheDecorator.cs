
using RedisApp.API.Models;
using RedisApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisApp.API.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _repository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProductRepositoryWithCacheDecorator(IProductRepository repository, RedisService redisService)
        {
            _repository = repository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDatabase(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
           var newProduct = await _repository.CreateAsync(product);

            if (_cacheRepository.KeyExists(productKey)) 
            {
                await _cacheRepository.HashSetAsync(productKey , product.Id , JsonSerializer.Serialize(newProduct));
            
            }
            return newProduct;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            if (! await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();

            var cacheProduct = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProduct.ToList()) 
            {
                var datas = JsonSerializer.Deserialize<Product>(item.Value);

                products.Add(datas);
            
            }
            return products;
        }

        public async Task<Product> GetAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey)) 
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync() 
        {
           var products = await _repository.GetAllAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey , p.Id , JsonSerializer.Serialize(p));
            });

            return products;
        
        }
    }    

}
