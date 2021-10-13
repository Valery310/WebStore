using System.Collections.Generic;
using WebStore.Domain.ViewModel;

namespace WebStore.Domain.Dto.Order
{
    public class CreateOrderModel
    {
        public OrderViewModel OrderViewModel { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
