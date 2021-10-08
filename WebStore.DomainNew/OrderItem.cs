using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain
{
    public class OrderItem : BaseEntity
    {
        public virtual Order Order { get; set; }
        [Required]
        public virtual Product Product { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [NotMapped] 
        public decimal TotalItemPrice => Price * Quantity;
    }
}
