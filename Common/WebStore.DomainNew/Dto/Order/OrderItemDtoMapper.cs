using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModel;

namespace WebStore.Domain.Dto.Order
{
    public static class OrderItemDtoMapper
    {
        public static OrderItemDto ToDTO(this WebStore.Domain.Entities.OrderItem order) => order is null ? null : new OrderItemDto
        {
            Id = order.Id,
            Price = order.Price,
            ProductId = order.Product.Id,
            Product = order.Product.ToDTO(),
            Quantity = order.Quantity,             
        };

        public static WebStore.Domain.Entities.OrderItem FromDTO(this OrderItemDto order) => order is null ? null :
            new WebStore.Domain.Entities.OrderItem
            {
                Id = order.Id,
                Price = order.Price,
                Product = order.Product.FromDTO(),
                Quantity = order.Quantity,
                
            };

        public static IEnumerable<OrderItemDto> ToDTO(this IEnumerable<WebStore.Domain.Entities.OrderItem> orders) => orders.Select(ToDTO);

        public static IEnumerable<WebStore.Domain.Entities.OrderItem> FromDTO(this IEnumerable<OrderItemDto> orders) => orders.Select(FromDTO);

        public static IEnumerable<OrderItemDto> ToDTO(this CartViewModel Cart) =>
            Cart.Items.Select(p => new OrderItemDto
            {
                ProductId = p.Key.Id,
                Price = p.Key.Price,
                Quantity = p.Value
            });

        //  public static IEnumerable<OrderItemDto> ToDTO(this ICollection<WebStore.Domain.Entities.OrderItem> orders) => orders.Select(ToDTO);

        //  public static ICollection<WebStore.Domain.Entities.OrderItem> FromDTO(this IEnumerable<OrderItemDto> orders) => (ICollection<Entities.OrderItem>)orders.Select(FromDTO);
    }
}
