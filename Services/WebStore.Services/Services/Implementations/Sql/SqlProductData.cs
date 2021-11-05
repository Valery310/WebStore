using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        ILogger<SqlProductData> _logger;

        public SqlProductData(WebStoreContext context, ILogger<SqlProductData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Section>> GetSections()
        {
            return await _context.Sections.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            return await _context.Brands.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Section> GetSectionsById(int id)
        {
            return await _context.Sections.SingleOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);
        }

        public async Task<Brand> GetBrandsById(int id)
        {
            return await _context.Brands.SingleOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);
        }

        public async Task<PageProduct> GetProducts(ProductFilter filter)
        {
            var query = _context.Products.Include("Section").Include("Brand").Where(p => !p.IsDelete).AsQueryable();

            if (filter.BrandId.HasValue)
            {
                query = query.Where(c => c.BrandId.HasValue && c.BrandId.Value.Equals(filter.BrandId.Value));
            }

            if (filter.SectionId.HasValue)
            {
                query = query.Where(c => c.SectionId.Equals(filter.SectionId.Value));
            }

            var model = new PageProduct
            {
                TotalCount = query.Count()
            };

            if (filter.PageSize.HasValue)
            {
                model.Products = await query.OrderBy(c => c.Order).Skip(((filter.Page - 1) * filter.PageSize.Value)).Take(filter.PageSize.Value)
                    .Select(p =>
                    new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Order = p.Order,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Brand = p.BrandId.HasValue ? new Brand()
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name
                        } : null,
                        Section = new Section()
                        {
                            Id = p.SectionId,
                            Name = p.Section.Name
                        }
                    }).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                model.Products = await query.OrderBy(c => c.Order).Select(p =>
                new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Brand = p.BrandId.HasValue ? new Brand()
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name
                    } : null,
                    Section = new Section()
                    {
                        Id = p.SectionId,
                        Name = p.Section.Name
                    }
                }).ToListAsync().ConfigureAwait(false);
            }
            return model;
        }

        public async Task<Product> GetProductById(int id)
        {
            _logger.LogInformation("Получение товара по id = {0}", id);

            var product = await _context.Products.Include("Brand").Include("Section").FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (product == null)
            {
                _logger.LogInformation("Товар {0} не найден", id);
                return null;
            }

            var dto = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price,
                Section = new Section()
                {
                    Id = product.SectionId,
                    Name = product.Section.Name
                }
            };

            if (product.Brand != null)
            {
                dto.Brand = new Brand()
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                };
            }

            _logger.LogInformation("Товар {0} найден", id);
            return dto;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            if (product is null)
            {
                _logger.LogInformation("Товар {0} не найден", product.Id);
                return -1;
            }
            using (await _context.Database.BeginTransactionAsync())
            {
                Product p = await _context.Products.FindAsync(product.Id);

                if (!(p is null))
                {
                    p.Name = product.Name;
                     p.Price = product.Price;
                    await _context.SaveChangesAsync();
             
                    await _context.Database.CommitTransactionAsync();
                    _logger.LogInformation("Товар {0} отредактирован", product.Id);
                    return p.Id;
                }
            }
            _logger.LogInformation("Товар {0} не найден", product.Id);
            return -1;
                
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Удаление товара по id = {0}", id);

            using (await _context.Database.BeginTransactionAsync()) 
            {
                var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (productToDelete != null)
                {
                   _context.Products.Remove(productToDelete);
                   await _context.SaveChangesAsync();
                   await _context.Database.CommitTransactionAsync();
                   _logger.LogInformation("Товар {0} удален", id);
                    return true;
                }
                else
                {
                    _logger.LogInformation("Товар {0} не найден", id);
                    return false;
                }
            }
                
        }

        public async Task<SaveResult> CreateProduct(Product product)
        {
            try
            {
                var _product = new Product()
                {
                    BrandId = product.Brand?.Id,
                    SectionId = product.Section.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Order = product.Order,
                    Price = product.Price,
                  //  Brand = product.Brand,
                 //   Section = product.Section,
                    IsDelete = product.IsDelete,
                };

                await _context.Products.AddAsync(_product).ConfigureAwait(false);
                await _context.SaveChangesAsync();

                return new SaveResult
                {
                    IsSuccess = true
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        
        }

        public async Task<SaveResult> UpdateProduct(Product product)
        {
            var _product = await _context.Products.FirstOrDefaultAsync().ConfigureAwait(false);
            if (_product == null)
            {
                return new SaveResult()
                {
                    IsSuccess = false,
                    Errors = new List<string>() { "Entity not exist" }
                };
            }
            _product.BrandId = product.Brand.Id;
            _product.SectionId = product.Section.Id;
            _product.ImageUrl = product.ImageUrl;
            _product.Order = product.Order;
            _product.Price = product.Price;
            _product.Name = product.Name;
            try
            {
                await _context.SaveChangesAsync();
                return new SaveResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }

            public async Task<SaveResult> DeleteProduct(int productId)
            {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(false);
            if (product == null)
                {
                    return new SaveResult()
                    {
                        IsSuccess = false,
                        Errors = new List<string>() { "Entity not exist" }
                    };
                }
                try
                {
                    //_context.Remove(product);
                    product.IsDelete = true;
                     _context.Update(product);
                    await _context.SaveChangesAsync();
                    return new SaveResult()
                    {
                        IsSuccess = true
                    };
                }
                catch (Exception e)
                {
                    return new SaveResult
                    {
                        IsSuccess = false,
                        Errors = new List<string>()
                    {
                        e.Message
                    }
                    };
                }
            }       
    }
}