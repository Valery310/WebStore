using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductData productData, IConfiguration configuration) 
        {
            _productData = productData;
            _configuration = configuration;
        }

        public async Task<IActionResult> Shop(int? sectionId, int? brandId, int page = 1)
        {
            var ps = int.Parse(_configuration["PageSize"]);
            var products = await _productData.GetProducts(new ProductFilter { BrandId = brandId, SectionId = sectionId, Page = page, PageSize = int.Parse(_configuration["PageSize"]) }).ConfigureAwait(false);

            var model = new CatalogViewModel()
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.Products.Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
             //       Brand = p.Brand != null ? p.Brand.Name : string.Empty
                }).OrderBy(p => p.Order).ToList(),
                PageViewModel = new PageViewModel
                {
                    PageSize = int.Parse(_configuration["PageSize"]),
                    PageNumber = page,
                    TotalItems = products.TotalCount
                }
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
                Brand = product.Brand,
                Section = product.Section
              //  Brand = product.Brand != null ? product.Brand.Name : string.Empty
            });

        }

        public async Task<IActionResult> GetFilteredItems(int? sectionId, int? brandId, int page = 1)
        {
            // var productsModel = GetProducts(sectionId, brandId, page, out var totalCount);
            var productsModel = await GetProducts(sectionId, brandId, page).ConfigureAwait(false);
            return PartialView("Partial/_ProductItems", productsModel);
        }


        //private IEnumerable<ProductViewModel> GetProducts(int? sectionId, int? brandId, int page, out int totalCount)
        private async Task<IEnumerable<ProductViewModel>> GetProducts(int? sectionId, int? brandId, int page)
        {
            var products = await _productData.GetProducts(new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = int.Parse(_configuration["PageSize"])
            }).ConfigureAwait(false);
           // totalCount = products.TotalCount;
            return products.Products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                Brand = p.Brand,                
            }).ToList().OrderBy(p => p.Order);
        }

    }
}
