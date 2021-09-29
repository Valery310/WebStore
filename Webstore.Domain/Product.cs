using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webstore.Domain.Entities.Base;
using Webstore.Domain.Entities.Base.Interfaces;

namespace Webstore.Domain
{
    /// <inheritdoc cref="NamedEntity" />
    /// <summary>
    /// Сущность продукт
    /// </summary>
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        /// <summary>
        /// Секция, к которой принадлежит товар
        /// </summary>
        public int SectionId { get; set; }
        /// <summary>
        /// Бренд товара
        /// </summary>
        public int? BrandId { get; set; }
        /// <summary>
        /// Ссылка на картинку
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
    }

}
