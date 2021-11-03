using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Services.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIAddresses.Products)
        {
         //   ServiceAddress = "api/products";
        }

      //  protected sealed override string ServiceAddress { get; set; }

        public async Task<IEnumerable<Section>> GetSections()
        {
            var url = $"{ServiceAddress}/sections";
            var result = Get<IEnumerable<SectionDto>>(url).FromDTO();
            return result;
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            var url = $"{ServiceAddress}/brands";
            var result = Get<IEnumerable<BrandDto>>(url).FromDTO();
            return result;
        }

        public async Task<PageProduct> GetProducts(ProductFilter filter)
        {
            var url = $"{ServiceAddress}";
            var response = Post(url, filter);
            var result = await response.Content.ReadAsAsync<PageProductDto>();
            return result.FromDTO();
        }

        public async Task<Product> GetProductById(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = Get<ProductDto>(url);
            return result.FromDTO();
        }

        public async Task<int> UpdateAsync(Product product)
        {
            var url = $"{ServiceAddress}/{product.Id}";
            var response = Put(url, product);
            var result = response.Content.ReadAsAsync<ProductDto>().Result;
            return result.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = Delete(url);
            return result.Content.ReadAsAsync<bool>().Result;
            // return result.Content.ReadAsAsync<bool>().Result;
        }

        public async Task<Section> GetSectionsById(int id)
        {
            var url = $"{ServiceAddress}/sections/{id}";
            var result = Get<SectionDto>(url).FromDTO();
            return result;
        }

        public async Task<Brand> GetBrandsById(int id)
        {
            var url = $"{ServiceAddress}/brands/{id}";
            var result = Get<BrandDto>(url).FromDTO();
            return result;
        }

        public async Task<SaveResult> CreateProduct(Product product)
        {
            var url = $"{ServiceAddress}/create";
            var response = Post(url, product);
            var result = await response.Content.ReadAsAsync<SaveResult>();
            return result;
        }

        public async Task<SaveResult> UpdateProduct(Product product)
        {
            var url = $"{ServiceAddress}";
            var response = Put(url, product);
            var result = await response.Content.ReadAsAsync<SaveResult>();
            return result;
        }

        public async Task<SaveResult> DeleteProduct(int productId)
        {
            var url = $"{ServiceAddress}/{productId}";
            var response = await DeleteAsync(url);
            var result = await response.Content.ReadAsAsync<SaveResult>();
            return result;
        }
    }

}
