using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Areas.Admin.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly IProductData _productData;

        public HomeController(IProductData productData) => _productData = productData;

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult ProductList() 
        //{
        //    var products = _productData.GetProducts(new Services.Filters.ProductFilter());
        //    return View(products);
        //}
    }
}
