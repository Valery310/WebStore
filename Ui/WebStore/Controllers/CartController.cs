using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Details()
        {
            var model = new DetailsViewModel()
            {
                Cart = await _cartService.TransformCart(),
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
            return Json(new { id, message = "Товар добавлен в корзину" });
         //   return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult GetCartView()
        {
            return ViewComponent("Cart");
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(OrderViewModel model, [FromServices] IOrdersService ordersService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new DetailsViewModel
                {
                    Cart = _cartService.TransformCart().Result,
                    Order = model
                });

            CreateOrderDto orderModel = new() {Items = _cartService.TransformCart().Result.ToDTO(), OrderModel = model };
            var order = await ordersService.CreateOrderAsync(orderModel, User.Identity!.Name );


            _cartService.RemoveAll();

            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}