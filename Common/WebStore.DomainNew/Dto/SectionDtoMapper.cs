using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Dto
{
    public static class SectionDtoMapper
    {
        public static SectionDto ToDTO(this Section section) => section is null ? null :
        new SectionDto
        {
            Id = section.Id,
            Name = section.Name,
            Order = section.Order,
            ParentId = section.ParentId,
        };

        public static Section FromDTO(this SectionDto section) => section is null ? null :
            new Section
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId = section.ParentId,
            };

        public static IEnumerable<SectionDto> ToDTO(this IEnumerable<Section> sections) => sections.Select(ToDTO);

        public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDto> sections) => sections.Select(FromDTO);
    }
}
