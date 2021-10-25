using System;
using Webstore.Domain.Entities.Base.Interfaces;
using Webstore.Domain.Entities.Base;

namespace Webstore.Domain
{
    public class Section: NamedEntity, IOrderedEntity
    {
        public int? ParentId { get; set; }
        public int Order { get; set; }
    }
}
