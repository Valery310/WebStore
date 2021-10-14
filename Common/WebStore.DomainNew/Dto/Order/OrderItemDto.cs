using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Dto.Order
{
    public class OrderItemDto : BaseEntity
    {
       // public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
        //public virtual OrderDto Order { get; set; }
        //public virtual ProductDto Product { get; set; }
        //public decimal Price { get; set; }
        //public int Quantity { get; set; }
        //public decimal TotalItemPrice => Price * Quantity;
    }
}
