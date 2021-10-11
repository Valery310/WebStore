using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Filters;
using WebStore.Services.Interfaces;
using WebStore.ViewModel;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData) 
        {
            _productData = productData;
        }

        public IActionResult Shop(int? sectionId, int? brandId)
        {
            var products = _productData.GetProducts(new ProductFilter { BrandId = brandId, SectionId = sectionId });

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
                    Price = p.Price
                }).OrderBy(p => p.Order).ToList()
            };

            return View(model);
        }


        public IActionResult ProductDetails()
        {
            return View();

        }
    }
}
