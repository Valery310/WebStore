using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;
        
        public ProductsController(IProductData productData) => _productData = productData;
            
        public IActionResult Index()
        {
            var products = _productData.GetProducts(new Services.Filters.ProductFilter());
            return View(products);
        }
    }
}
