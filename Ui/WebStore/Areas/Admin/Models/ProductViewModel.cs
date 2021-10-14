using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Areas.Admin.Models
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public Brand Brand { get; set; }
        public Section Section { get; set; }
    }
}
