using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IOrdersService _ordersService;
        public ProfileController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Orders()
        { 
            var orders = await _ordersService.GetUserOrdersAsync(User.Identity.Name);
            var orderModels = new List<UserOrderViewModel>(orders.Count());
            foreach (var order in orders)
            {
                orderModels.Add(new UserOrderViewModel()
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    TotalSum = order.OrderItems.Sum(o => o.Price * o.Quantity),
                    Description = order.Description,
                    Date = order.Date
                });
            }
            return View(orderModels);
        }

    }
}
