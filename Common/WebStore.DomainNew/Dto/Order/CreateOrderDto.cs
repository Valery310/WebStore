using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;

namespace WebStore.Domain.Dto.Order
{
    public class CreateOrderDto
    {
        // public OrderViewModel OrderViewModel { get; set; }
        // public List<OrderItem> OrderItems { get; set; }
        public OrderViewModel OrderModel { get; set; }
        public IEnumerable<OrderItemDto> Items {get;set;}
     //  public CartViewModel Cart { get; set; }
    }
}
