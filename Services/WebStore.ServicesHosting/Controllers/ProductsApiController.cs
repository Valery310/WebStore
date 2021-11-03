using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route(WebAPIAddresses.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _productData;
        private readonly ILogger<ProductsApiController> _logger;

        public ProductsApiController(IProductData productData, ILogger<ProductsApiController> logger)
        {
            _productData = productData;
            _logger = logger;
        }

        [HttpGet("sections")]
        public async Task<IActionResult> GetSections()
        {
            var result = await _productData.GetSections();
            return result is null ? NotFound() : Ok(result.ToDTO());
        }

        [HttpGet("sections/{id}")]
        public async Task<IActionResult> GetSections(int id)
        {
            var result = await _productData.GetSectionsById(id);
            return result is null ? NotFound():Ok(result.ToDTO());
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var result = await _productData.GetBrands();
            return result is null ? NotFound() : Ok(result.ToDTO());
        }

        [HttpGet("brands/{id}")]
        public async Task<IActionResult> GetBrands(int id)
        {
            var result = await _productData.GetBrandsById(id);
            return result is null ? NotFound() : Ok(result.ToDTO()); ;
        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<IActionResult> GetProducts([FromBody]ProductFilter filter)
        {
            var result = await _productData.GetProducts(filter);
            return result is null ? NotFound() : Ok(result.ToDTO()); ;
        }

        [HttpGet("{id}"), ActionName("Get")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productData.GetProductById(id);
            return product is null ? NotFound() : Ok(product.ToDTO());
        }

        [HttpPut("{id}"), ActionName("Put")]
        public async Task<IActionResult> UpdateAsync([FromBody] Product product)
        {
            var result = await _productData.UpdateAsync(product);
            return result > 0 ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _productData.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpPost("create")]
        public async Task<SaveResult> CreateProduct([FromBody] Product product)
        {
            var result = await _productData.CreateProduct(product);
            return result;
        }
        [HttpPut]
        public async Task<SaveResult> UpdateProduct([FromBody] Product product)
        {
            var result = await _productData.UpdateProduct(product);
            return result;
        }
        [HttpDelete("{productId}")]
        public async Task<SaveResult> DeleteProduct(int productId)
        {
            var result = await _productData.DeleteProduct(productId);
            return result;
        }
    }
}
