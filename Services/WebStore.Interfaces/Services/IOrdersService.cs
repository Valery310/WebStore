using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userName);
        Task<Order> GetOrderByIdAsync(int id);
        // Task<OrderDto> CreateOrderAsync(CreateOrderModel orderModel, string userName);
        Task<Order> CreateOrderAsync(CreateOrderDto orderModel, string userName);
    }

}
