using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Dto
{
    public static class PageProductDtoMapper
    {
        public static PageProductDto ToDTO(this PageProduct product) => product is null ? null : new PageProductDto
        {
            Products = product.Products.ToDTO(),
            TotalCount = product.TotalCount
        };

        public static PageProduct FromDTO(this PageProductDto product) => product is null ? null :
            new PageProduct
            {
                Products = product.Products.FromDTO(),
                TotalCount = product.TotalCount        
            };

        public static IEnumerable<PageProductDto> ToDTO(this IEnumerable<PageProduct> pageproducts) => pageproducts.Select(ToDTO);

        public static IEnumerable<PageProduct> FromDTO(this IEnumerable<PageProductDto> pageproducts) => pageproducts.Select(FromDTO);
    }
}
