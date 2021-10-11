using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Areas.Admin.Models;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;
        
        public ProductsController(IProductData productData) => _productData = productData;
            
        public IActionResult Index()
        {
            var products = _productData.GetProducts(new ProductFilter());
            return View(products);
        }

     //   public IActionResult Edit(int id) => RedirectToAction(nameof(Index));

     //   public IActionResult Delete(int id) => RedirectToAction(nameof(Index));

        [Route("edit/{id?}")]
        public IActionResult Edit(int id)
        {
            ProductViewModel model;

                var temp = _productData.GetProductById(id);
                model = new ProductViewModel
                {
                    Id = temp.Id,
                    Name = temp.Name,
                    Order = temp.Order,
                    ImageUrl = temp.ImageUrl,
                    Price = temp.Price,
                    Brand = temp.Brand,
                    Section = temp.Section
                };

                if (ReferenceEquals(model, null))
                {
                    return NotFound();
                }
            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]
        public IActionResult Edit(ProductViewModel model)
        {
            if (model.Price < 0)
            {
                ModelState.AddModelError("Price", "Цена не может быть меньше нуля.");
            }
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var dbitem = _productData.GetProductById(model.Id);

                    if (ReferenceEquals(dbitem, null))
                    {
                        return NotFound();
                    }
                    dbitem.Name = model.Name;
                    dbitem.ImageUrl = model.ImageUrl;
                    dbitem.Order = model.Order;
                    dbitem.Price = model.Price;
                    dbitem.Brand = model.Brand;
                    dbitem.Section = model.Section;

                    _productData.UpdateAsync(dbitem);
                }
                return RedirectToAction(nameof(Index));
            }
            //Возвращаем модель, если не валидна.
            return View(model);
        }

        [Route("delete/{id}")]
        public ActionResult Delete(int id)
        {
            _productData.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
