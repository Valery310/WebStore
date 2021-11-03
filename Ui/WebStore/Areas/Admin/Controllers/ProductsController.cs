using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;
        
        public ProductsController(IProductData productData) => _productData = productData;
            
        public async Task<IActionResult> Index()
        {
            var products = await _productData.GetProducts(new ProductFilter());
            return View(products);
        }

     //   public IActionResult Edit(int id) => RedirectToAction(nameof(Index));

     //   public IActionResult Delete(int id) => RedirectToAction(nameof(Index));

        [Route("edit/{id?}")]
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
                BrandId = product?.Brand.Id,
                SectionId = product.Section.Id,
                Brands = new SelectList(brands, "Id", "Name", product.Brand?.Id),
                Sections = new SelectList(notParentSections, "Id", "Name", product.Section.Id)
            });
        }

        [HttpPost]
        [Route("edit/{id?}")]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var tempSections = await _productData.GetSections();
            var notParentSections = tempSections.Where(s => s.ParentId != null);
            var brands = await _productData.GetBrands();

            if (ModelState.IsValid && model.SectionId != 0)
            {
                var productDto = new Product()
                {
                    Id = model.Id,
                    ImageUrl = model.ImageUrl,
                    Name = model.Name,
                    Order = model.Order,
                    Price = model.Price,
                    Brand = model.BrandId != -1 ?  brands?.Single(p => p.Id == model.BrandId): null,
                    Section = tempSections.Single(p => p.Id == model.SectionId)
                };
                if (model.Id > 0)
                {
                    await _productData.UpdateProduct(productDto);
                }
                else
                {
                    await _productData.CreateProduct(productDto);
                }
                //  return RedirectToAction("ProductList", "Home", new { area = "Admin" });
                return RedirectToAction(nameof(Index));
            }
            model.Brands = new SelectList(brands, "Id", "Name", model.Brands);
            model.Sections = new SelectList(notParentSections, "Id", "Name", model.Sections);
            return View(model);
        }

        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _productData.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
