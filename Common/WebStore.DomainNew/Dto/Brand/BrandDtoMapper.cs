using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Dto
{
    public static class BrandDtoMapper
    {
        public static BrandDto ToDTO(this Brand brand) => brand is null ? null : new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order,     
        };

        public static Brand FromDTO(this BrandDto brand) => brand is null ? null :
            new Brand
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
            };

        public static IEnumerable<BrandDto> ToDTO(this IEnumerable<Brand> brands) => brands.Select(ToDTO);

        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDto> brands) => brands.Select(FromDTO);

    }
}
