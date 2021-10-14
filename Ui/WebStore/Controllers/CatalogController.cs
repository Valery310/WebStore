using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData) 
        {
            _productData = productData;
        }

        public async Task<IActionResult> Shop(int? sectionId, int? brandId)
        {
            var products = await _productData.GetProducts(new ProductFilter { BrandId = brandId, SectionId = sectionId }).ConfigureAwait(false);

            var model = new CatalogViewModel()
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
             //       Brand = p.Brand != null ? p.Brand.Name : string.Empty
                }).OrderBy(p => p.Order).ToList()
            };

            return View(model);
        }


        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productData.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductViewModel 
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
           //     Brand = product.Brand != null ? product.Brand.Name : string.Empty
            });

        }
    }
}
