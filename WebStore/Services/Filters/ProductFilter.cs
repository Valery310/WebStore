using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Services.Filters
{
    /// <summary>
    /// Класс для фильтрации товаров
    /// </summary>
    public class ProductFilter
    {
        /// <summary>
        /// Секция, к которой принадлежит товар
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Бренд товара
        /// </summary>
        public int? BrandId { get; set; }
    }
}
