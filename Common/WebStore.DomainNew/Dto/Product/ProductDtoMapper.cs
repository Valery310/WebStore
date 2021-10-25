using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Dto
{
    public static class ProductDtoMapper
    {
        public static ProductDto ToDTO(this Product product) => product is null ? null : new ProductDto
        {
           Id = product.Id,
           Name = product.Name,
           Order = product.Order,
           Price = product.Price,
           ImageUrl = product.ImageUrl,
           Brand = product.Brand.ToDTO(),
           Section = product.Section.ToDTO(),
        };

        public static Product FromDTO(this ProductDto product) => product is null ? null :
            new Product
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand.FromDTO(),
                Section = product.Section.FromDTO(),
            };

        public static IEnumerable<ProductDto> ToDTO(this IEnumerable<Product> products) => products.Select(ToDTO);

        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDto> products) => products.Select(FromDTO);
    }
}
