using System;
using WebStore.Domain.Entities.Base.Interfaces;
using WebStore.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebStore.Domain
{
    /// <inheritdoc cref="NamedEntity" />
    /// <summary>
    /// Сущность секция
    /// </summary>

    [Table("Section")]
    public class Section: NamedEntity, IOrderedEntity
    {
        /// <summary>
        /// Родительская секция (при наличии)
        /// </summary>
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Section ParentSection { get; set; }

        public int Order { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
