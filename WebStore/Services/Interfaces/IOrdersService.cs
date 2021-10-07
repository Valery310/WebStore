using System.Collections.Generic;
using WebStore.DomainNew;
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
