using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webstore.Domain.Entities.Base.Interfaces;

namespace Webstore.Domain.Entities.Base
{
    public class NamedEntity: INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
