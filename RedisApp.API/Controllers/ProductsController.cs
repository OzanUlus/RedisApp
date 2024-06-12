using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisApp.API.Models;
using RedisApp.API.Repository;
using RedisApp.Cache;

namespace RedisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
   
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id) 
        {
          var data = await _productRepository.GetAsync(id);
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct() 
        {
          var datas = await _productRepository.GetAllAsync();
            return Ok(datas);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product) 
        {
            var data = await _productRepository.CreateAsync(product);
            return Created(string.Empty,data);
        }
    }
}
