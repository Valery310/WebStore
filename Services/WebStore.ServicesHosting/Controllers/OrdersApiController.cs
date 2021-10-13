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
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrdersService
    {
        private readonly IOrdersService _ordersService;
        private readonly ILogger<OrdersApiController> _logger;

        public OrdersApiController(IOrdersService ordersService, ILogger<OrdersApiController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        [HttpGet("user/{userName}")]
        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userName)
        {
            return await _ordersService.GetUserOrdersAsync(userName);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            return await _ordersService.GetOrderByIdAsync(id);
        }

        [HttpPost("{userName?}")]
        public async Task<OrderDto> CreateOrderAsync([FromBody] CreateOrderModel orderModel, string userName)
        {
            return await _ordersService.CreateOrderAsync(orderModel, userName);
        }
    }
}
