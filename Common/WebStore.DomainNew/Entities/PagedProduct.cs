using System.Collections.Generic;

namespace WebStore.Domain.Entities
{
    public class PageProduct
    {
        /// <summary>
        /// Выборка продуктов для текущей страницы
        /// </summary>
        public IEnumerable<Product> Products { get; set; }
        /// <summary>
        /// Общее количество в запросе
        /// </summary>
        public int TotalCount { get; set; }
    }
}
