using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Services.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, "api/products")
        {
         //   ServiceAddress = "api/products";
        }

        protected sealed override string ServiceAddress { get; set; }

        public IEnumerable<Section> GetSections()
        {
            var url = $"{ServiceAddress}/sections";
            var result = Get<List<Section>>(url);
            return result;
        }

        public IEnumerable<Brand> GetBrands()
        {
            var url = $"{ServiceAddress}/brands";
            var result = Get<List<Brand>>(url);
            return result;
        }

        public IEnumerable<ProductDto> GetProducts(ProductFilter filter)
        {
            var url = $"{ServiceAddress}";
            var response = Post(url, filter);
            var result =
            response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
            return result;
        }

        public ProductDto GetProductById(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = Get<ProductDto>(url);
            return result;
        }

        public Task UpdateAsync(ProductDto product)
        {
            var url = $"{ServiceAddress}/{product.Id}";
            var response = Put(url, product);
            var result = response.Content.ReadAsAsync<ProductDto>().Result;
            return null;
        }

        public Task DeleteAsync(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = Delete(url);
            return null;
           // return result.Content.ReadAsAsync<bool>().Result;
        }
    }

}
