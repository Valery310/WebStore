using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

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

        public IEnumerable<ProductDto> GetProducts(ProductFilter filter) 
        {
            var query = _context.Products.Include("Section").Include("Brand").AsQueryable();

            if (filter.BrandId.HasValue)
            {
                query = query.Where(c => c.BrandId.HasValue && c.BrandId.Value.Equals(filter.BrandId.Value));
            }

            if (filter.SectionId.HasValue)
            {
                query = query.Where(c => c.SectionId.Equals(filter.SectionId.Value));
            }

            //if (filter.Ids != null && filter.Ids.Length > 0)
            //{
            //    List<Product> product = new List<Product>();
            //     foreach (var i in filter.Ids)
            //    {
            //         product.Add(query.FirstOrDefault(p => p.Id == i));
            //    }

            //    query = product.AsQueryable<Product>();
            //}

            //return query.ToList();

            return query.Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Brand = p.BrandId.HasValue ? new BrandDto()
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                } : null
            }).ToList();
        }

        public ProductDto GetProductById(int id) 
        {
            var product = _context.Products.Include("Brand").Include("Section").FirstOrDefault(p => p.Id.Equals(id));

            if (product == null)
            {
                return null;
            }

            var dto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price
            };

            if (product.Brand != null)
            {
                dto.Brand = new BrandDto()
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                };
            }

            return dto;

          //  return _context.Products.Include("Brand").Include("Section").FirstOrDefault(p => p.Id.Equals(id));
        }

        public async Task UpdateAsync(ProductDto product)
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