using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData _productData;
        private readonly ILogger<ProductsApiController> _logger;

        public ProductsApiController(IProductData productData, ILogger<ProductsApiController> logger)
        {
            _productData = productData;
            _logger = logger;
        }

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections()
        {
            return _productData.GetSections();
        }

        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()
        {
            return _productData.GetBrands();
        }

        [HttpPost]
        [ActionName("Post")]
        public IEnumerable<ProductDto> GetProducts([FromBody]ProductFilter filter)
        {
            return _productData.GetProducts(filter);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public ProductDto GetProductById(int id)
        {
            var product = _productData.GetProductById(id);
            return product;
        }

        [HttpPut("{id}"), ActionName("Put")]
        public Task UpdateAsync([FromBody] ProductDto product)
        {
            _productData.UpdateAsync(product);
            return null;
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            _productData.DeleteAsync(id);
            return null;
        }
    }
}
