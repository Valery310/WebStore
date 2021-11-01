using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Filters
{
    /// <summary>
    /// Класс для фильтрации товаров
    /// </summary>
    public class ProductFilter
    {
        public int[] Ids { get; set; }
        /// <summary>
        /// Секция, к которой принадлежит товар
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Бренд товара
        /// </summary>
        public int? BrandId { get; set; }
        /// <summary>
        /// Количество товара на странице
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// Страница товара
        /// </summary>
        public int Page { get; set; }
    }
}
