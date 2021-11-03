using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

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

        public async Task<IActionResult> ProductList()
        {
            var products = await _productData.GetProducts(new ProductFilter());
            return View(products);
        }
    }
}

