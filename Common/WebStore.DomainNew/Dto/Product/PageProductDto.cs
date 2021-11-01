using System.Collections.Generic;

namespace WebStore.Domain.Dto
{
    public class PageProductDto
    {
        /// <summary>
        /// Выборка продуктов для текущей страницы
        /// </summary>
        public IEnumerable<ProductDto> Products { get; set; }
        /// <summary>
        /// Общее количество в запросе
        /// </summary>
        public int TotalCount { get; set; }
    }
}
