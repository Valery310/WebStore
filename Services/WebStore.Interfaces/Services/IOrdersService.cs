using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;

namespace WebStore.Interfaces.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userName);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }
}
