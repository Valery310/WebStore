using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webstore.Domain.Entities.Base;
using Webstore.Domain.Entities.Base.Interfaces;

namespace Webstore.Domain
{
    public class Brand: NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
    }
}
