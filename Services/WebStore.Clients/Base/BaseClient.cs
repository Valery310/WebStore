using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    /// <summary>
    /// Базовый клиент
    /// </summary>
    public abstract class BaseClient
    {
        /// <summary>
        /// Http клиент для связи
        /// </summary>
        protected HttpClient _Client;
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        protected abstract string ServiceAddress { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация проекта</param>
        protected BaseClient(HttpClient Client, string Address)
        {
            _Client = Client;
            ServiceAddress = _Client.BaseAddress + Address;
            // Создаем экземпляр клиента
            _Client = new HttpClient
            {
                // Базовый адрес, на котором будут хостится сервисы
                //  BaseAddress = new Uri(configuration["ClientAdress"])
                BaseAddress = new Uri(ServiceAddress)
            };
            _Client.DefaultRequestHeaders.Accept.Clear();

            // Устанавливаем хедер, который будет говорит серверу, чтобы он отправлял данные в формате json
        Client.DefaultRequestHeaders.Accept.Add(new
        MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected T Get<T>(string url) where T : new()
        {
            var result = new T();
            var response = _Client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
                result = response.Content.ReadFromJsonAsync<T>().Result;//.ReadAsAsync<T>();
            return result;
        }
        protected async Task<T> GetAsync<T>(string url) where T : new()
        {
            var list = new T();
            var response = await _Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
                list = await response.Content.ReadFromJsonAsync<T>();//.ReadAsAsync<T>();
            return list;
        }
        protected HttpResponseMessage Post<T>(string url, T value)
        {
            var response = _Client.PostAsJsonAsync(url, value).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T
        value)
        {
            var response = await _Client.PostAsJsonAsync(url, value);
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected HttpResponseMessage Put<T>(string url, T value)
        {
            var response = _Client.PutAsJsonAsync(url, value).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T
        value)
        {
            var response = await _Client.PutAsJsonAsync(url, value);
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected HttpResponseMessage Delete(string url)
        {
            var response = _Client.DeleteAsync(url).Result;
            return response;
        }
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _Client.DeleteAsync(url);
            return response;
        }


    }
}
