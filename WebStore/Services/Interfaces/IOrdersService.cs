using System.Collections.Generic;
using WebStore.Domain;
using WebStore.ViewModel;

namespace WebStore.Services.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<Order> GetUserOrders(string userName);
        Order GetOrderById(int id);
        Order CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }

}
