using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.ViewModel;

namespace WebStore.Interfaces.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userName);
        Task<OrderDto> GetOrderByIdAsync(int id);
        // Task<OrderDto> CreateOrderAsync(CreateOrderModel orderModel, string userName);
        Task<OrderDto> CreateOrderAsyn(OrderViewModel OrderModel, CartViewModel Cart, string UserName);
    }

}
