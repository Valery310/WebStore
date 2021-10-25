using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Interfaces.Api;
using System.Net.Http.Json;

namespace WebStore.Clients.Services.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient client) : base(client, "api/Values") { }

     //   protected sealed override string ServiceAddress { get; set; }

        public IEnumerable<string> Get()
        {
            var response = _Client.GetAsync($"{ServiceAddress}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<string>>().Result;
            }
            return null;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = _Client.GetAsync($"{ServiceAddress}").Result;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<string>>();
            }
            return null;
        }

        public string Get(int id)
        {
            var result = string.Empty;
            var response = _Client.GetAsync($"{ServiceAddress}/get/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadFromJsonAsync<string>().Result;
            }
            return result;
        }

        public async Task<string> GetAsync(int id)
        {
            var result = string.Empty;
            var response = await _Client.GetAsync($"{ServiceAddress}/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<string>();
            }
            return result;
        }

        public Uri Post(string value)
        {
            var response = _Client.PostAsJsonAsync($"{ServiceAddress}/post",
            value).Result;
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task<Uri> PostAsync(string value)
        {
            var response = await
            _Client.PostAsJsonAsync($"{ServiceAddress}/post", value);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public HttpStatusCode Put(int id, string value)
        {
            var response = _Client.PutAsJsonAsync($"{ServiceAddress}/put/{id}",
            value).Result;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await
            _Client.PutAsJsonAsync($"{ServiceAddress}/put/{id}", value);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public HttpStatusCode Delete(int id)
        {
            var response =
            _Client.DeleteAsync($"{ServiceAddress}/delete/{id}").Result;
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            var response = await
            _Client.DeleteAsync($"{ServiceAddress}/delete/{id}");
            return response.StatusCode;
        }

        public string GetCount()
        {
            var response = _Client.GetAsync($"{ServiceAddress}/count").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<string>().Result;
            }
            return null;
        }

        public async Task<string> GetCountAsync()
        {
            var response = _Client.GetAsync($"{ServiceAddress}/count").Result;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<string>();
            }
            return null;
        }
    }

}
