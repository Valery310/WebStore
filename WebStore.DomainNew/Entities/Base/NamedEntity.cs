using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public class NamedEntity: BaseEntity, INamedEntity
    {
       // public int Id { get; set; }
        public string Name { get; set; }
    }
}
