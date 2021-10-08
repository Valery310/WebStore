using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.Services.Filters;
using Microsoft.EntityFrameworkCore;
using WebStore.Areas.Admin.Models;
using System;
using System.Threading.Tasks;

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
            var query = _context.Products.Include("Section").AsQueryable().Include("Brand").AsQueryable();

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

        public async Task UpdateAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            using (await _context.Database.BeginTransactionAsync())
            {
                Product p = await _context.Products.FindAsync(product.Id);

                if (!(p is null))
                {
                    //  _context.Database.ExecuteSqlRaw($"UPDATE [dbo].[Products] SET Price = {product.Price}, Name = \'{product.Name}\' WHERE Id = {product.Id}");
                    p.Name = product.Name;
                     p.Price = product.Price;
                  //  _context.Products.Update(p);
                    await _context.SaveChangesAsync();
             
                    await _context.Database.CommitTransactionAsync();
                }
            }
                
        }

        public async Task DeleteAsync(int id)
        {
            using (await _context.Database.BeginTransactionAsync()) 
            {
                var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (productToDelete != null)
                {
                   _context.Products.Remove(productToDelete);
                  // _context.Products.Remove(await _context.Products.FindAsync(id));
                   await _context.SaveChangesAsync();
                   await _context.Database.CommitTransactionAsync();
                }
                else
                {
                    throw new ArgumentNullException(nameof(id));
                }
            }
                
        }
    }
}