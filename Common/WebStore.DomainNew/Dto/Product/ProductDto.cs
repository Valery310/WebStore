using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Dto
{
    public class ProductDto : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public BrandDto Brand { get; set; }
        public SectionDto Section { get; set; }
    }
}
