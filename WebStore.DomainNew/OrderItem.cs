using WebStore.Domain;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain
{
    public class OrderItem : BaseEntity
    {
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
