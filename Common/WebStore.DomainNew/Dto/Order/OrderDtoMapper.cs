using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Dto.Order
{
    public static class OrderDtoMapper
    {
        public static OrderDto ToDTO(this WebStore.Domain.Entities.Order order) => order is null ? null : 
            new OrderDto
        {
           Id = order.Id,
           Name = order.Name,
           Address = order.Address,
           Date = order.Date,
           Phone = order.Phone,
           Description = order.Description,
           Items = order.OrderItems.ToDTO().ToList(),
        };

        public static WebStore.Domain.Entities.Order FromDTO(this OrderDto order) => order is null ? null :
            new WebStore.Domain.Entities.Order
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Date = order.Date,
                Phone = order.Phone,
                Description = order.Description,
                OrderItems = order.Items.FromDTO().ToList(),
            };

        public static IEnumerable<OrderDto> ToDTO(this IEnumerable<WebStore.Domain.Entities.Order> orders) => orders.Select(ToDTO);

        public static IEnumerable<WebStore.Domain.Entities.Order> FromDTO(this IEnumerable<OrderDto> orders) => orders.Select(FromDTO);
    }
}
