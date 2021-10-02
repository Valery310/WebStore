using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.Services.Filters;

namespace WebStore.Services.Implementations.Sql
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _context;

        public SqlProductData(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Section> GetSections()
        {
            return _context.Sections.ToList();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            var query = _context.Products.AsQueryable();
            if (filter.BrandId.HasValue)
            {
                query = query.Where(c => c.BrandId.HasValue &&
                    c.BrandId.Value.Equals(filter.BrandId.Value));
            }

            if (filter.SectionId.HasValue)
            {
                query = query.Where(c =>
                   c.SectionId.Equals(filter.SectionId.Value));
            }
        
            return query.ToList();
        }
    }
}