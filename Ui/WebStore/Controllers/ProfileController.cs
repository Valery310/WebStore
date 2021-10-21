using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IOrdersService ordersService, ILogger<ProfileController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Запрос домашней страницы профиля пользователя {0}", User.Identity.Name);
            return View();
        }
        public async Task<IActionResult> Orders()
        {
            _logger.LogInformation("Получение списка заказов пользователя {0}", User.Identity.Name);
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
            _logger.LogInformation("Получение списка заказов получен");
            return View(orderModels);
        }

    }
}
