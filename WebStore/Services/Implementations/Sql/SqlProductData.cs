using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.Services.Filters;
using Microsoft.EntityFrameworkCore;

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
            var query = _context.Products.Include("Section").AsQueryable();

            if (filter.BrandId.HasValue)
            {
                query = query.Where(c => c.BrandId.HasValue && c.BrandId.Value.Equals(filter.BrandId.Value));
            }

            if (filter.SectionId.HasValue)
            {
                query = query.Where(c => c.SectionId.Equals(filter.SectionId.Value));
            }

            if (filter.Ids != null && filter.Ids.Length > 0)
            {
                List<Product> product = new List<Product>();
                 foreach (var i in filter.Ids)
                {
                     product.Add(query.FirstOrDefault(p => p.Id == i));
                }

                query = product.AsQueryable<Product>();
            }

            return query.ToList();
        }

        public Product GetProductById(int id) 
        {
            return _context.Products.Include("Brand").Include("Section").FirstOrDefault(p => p.Id.Equals(id));
        }
    }
}