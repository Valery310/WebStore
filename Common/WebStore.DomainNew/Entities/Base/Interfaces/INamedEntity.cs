using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities.Base.Interfaces
{
    public interface INamedEntity: IBaseEntity
    {
        string Name { get; set; }
    }
}
