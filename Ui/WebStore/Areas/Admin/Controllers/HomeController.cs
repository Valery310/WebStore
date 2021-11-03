using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Areas.Admin.Models;
using WebStore.Domain.Entities;
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

        public IActionResult ProductList()
        {
            var products = _productData.GetProducts(new ProductFilter());
            return View(products);
        }

        public async Task<IActionResult> EditAsync(int? id)
        {
            var tempSections = await _productData.GetSections().ConfigureAwait(false);
            var notParentSections = tempSections.Where(s => s.ParentId != null);
            var brands = await _productData.GetBrands();

            if (!id.HasValue)
            {
                return View(new ProductViewModel()
                {
                    Sections = new SelectList(notParentSections, "Id", "Name"),
                    Brands = new SelectList(brands, "Id", "Name")
                });
            }
            var product = await _productData.GetProductById(id.Value);

            if (product == null)
            {
                return NotFound(); 
            }
                
            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Section = product.Section,
                Brand = product?.Brand,
                Brands = new SelectList(brands, "Id", "Name", product.Brand?.Id),
                Sections = new SelectList(notParentSections, "Id", "Name", product.Section.Id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var tempSections = await _productData.GetSections();
            var notParentSections = tempSections.Where(s => s.ParentId != null);
            var brands = await _productData.GetBrands();

            if (ModelState.IsValid)
            {
                var productDto = new Product()
                {
                    Id = model.Id,
                    ImageUrl = model.ImageUrl,
                    Name = model.Name,
                    Order = model.Order,
                    Price = model.Price,
                    Brand = model.Brand,
                    Section = model.Section
                };
                if (model.Id > 0)
                {
                    await _productData.UpdateProduct(productDto);
                }
                else
                {
                    await _productData.CreateProduct(productDto);
                }
                return RedirectToAction(nameof(ProductList));
            }
            model.Brands = new SelectList(brands, "Id", "Name", model.Brands);
            model.Sections = new SelectList(notParentSections, "Id", "Name", model.Sections);
            return View(model);
        }
    }
}

