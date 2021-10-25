using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Dto;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly ILogger<OrdersApiController> _logger;

        public OrdersApiController(IOrdersService ordersService, ILogger<OrdersApiController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        [HttpGet("user/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
        public async Task<IActionResult> GetUserOrdersAsync(string userName)
        {
            var orders = await _ordersService.GetUserOrdersAsync(userName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id:int}"), ActionName("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var order = await _ordersService.GetOrderByIdAsync(id);
            return Ok(order.ToDTO());
        }

        [HttpPost("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto orderModel, string userName)
        {
            var order = _ordersService.CreateOrderAsync(orderModel, userName).Result;
            return Ok(order.ToDTO());
          //  return _ordersService.CreateOrderAsync(orderModel, userName);
        }
    }
}
