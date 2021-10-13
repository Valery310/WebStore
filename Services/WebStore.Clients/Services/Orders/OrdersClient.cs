using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Dto.Order;
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

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userName)
        {
            var url = $"{ServiceAddress}/user/{userName}";
            var result = await GetAsync<List<OrderDto>>(url);
            return result;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = await GetAsync<OrderDto>(url);
            return result;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderModel orderModel, string userName)
        {
            var url = $"{ServiceAddress}/{userName}";
            var response = await PostAsync(url, orderModel);
            var result = await response.Content.ReadAsAsync<OrderDto>();
            return result;
        }
    }

}
