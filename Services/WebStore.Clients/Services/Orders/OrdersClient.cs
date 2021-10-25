using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;


namespace WebStore.Clients.Services.Orders
{
    public class OrdersClient : BaseClient, IOrdersService
    {
        public OrdersClient(HttpClient client) : base(client, "api/orders")
        {
           // ServiceAddress = "api/orders";
        }

        protected sealed override string ServiceAddress { get; set; }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName)
        {
            var url = $"{ServiceAddress}/user/{userName}";
            var result = await GetAsync<IEnumerable<OrderDto>>(url).ConfigureAwait(false);
            return result.FromDTO();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = await GetAsync<OrderDto>(url);
            return result.FromDTO();
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto orderModel, string userName)
        //public async Task<Order> CreateOrderAsync(OrderViewModel OrderModel, CartViewModel Cart, string userName)
        {
            var url = $"{ServiceAddress}/{userName}";
            var response = Post(url, orderModel);
            //    var response = await PostAsync(url, orderModel);
            var result = response.Content.ReadAsAsync<OrderDto>().Result;
            return result.FromDTO();
        }
    }

}
