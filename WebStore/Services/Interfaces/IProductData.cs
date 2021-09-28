using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webstore.Domain;

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();
    }
}
