using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Dto.Order
{
    public class OrderItemDto : BaseEntity
    {
        public virtual OrderDto Order { get; set; }
        [Required]
        public virtual ProductDto Product { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [NotMapped]
        public decimal TotalItemPrice => Price * Quantity;
    }
}
