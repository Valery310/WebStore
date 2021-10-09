using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModel;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrdersService _ordersService;

        public CartController(ICartService cartService, IOrdersService
        ordersService)
        {
            _cartService = cartService;
            _ordersService = ordersService;
        }

        public IActionResult Details()
        {
            var model = new DetailsViewModel()
            {
                Cart = _cartService.TransformCart(),
                Order = new OrderViewModel()
            };
            return View(model);
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int id, string returnUrl)
        {
            _cartService.AddToCart(id);
            return Redirect(returnUrl);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(OrderViewModel model, [FromServices] IOrdersService ordersService)
        {
            if (ModelState.IsValid)
            {
                var orderResult = await _ordersService.CreateOrderAsync(model,
                _cartService.TransformCart(), User.Identity.Name);
                _cartService.RemoveAll();
                return RedirectToAction("OrderConfirmed", new
                {
                    id = orderResult.Id
                });
            }

            var detailsModel = new DetailsViewModel()
            {
                Cart = _cartService.TransformCart(),
                Order = model
            };

            return View("Details", detailsModel);
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}