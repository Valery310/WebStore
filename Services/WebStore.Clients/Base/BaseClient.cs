using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    /// <summary>
    /// Базовый клиент
    /// </summary>
    public abstract class BaseClient: IDisposable
    {
        /// <summary>
        /// Http клиент для связи
        /// </summary>
        protected HttpClient _Client { get; }
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        protected string ServiceAddress { get; }
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

        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default) //where T : new()
        {
            // var response = await _Client.GetAsync(url, Cancel).ConfigureAwait(false);
            var response =  _Client.GetAsync(url).Result;
            if (response.StatusCode == HttpStatusCode.NoContent) return default;
            return response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>().Result;
  
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _Client.PostAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        //protected HttpResponseMessage Put<T>(string url, T value)
        //{
        //    var response = _Client.PutAsJsonAsync(url, value).Result;
        //    response.EnsureSuccessStatusCode();
        //    return response;
        //}

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T value, CancellationToken Cancel = default)
        {
            var response = await _Client.PutAsJsonAsync(url, value).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            var response = await _Client.DeleteAsync(url, Cancel).ConfigureAwait(false);
            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        private bool _Disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed) return;
            _Disposed = true;

            if (disposing)
            {
                // должны освободить управляемые ресурсы
                //Http.Dispose(); - вызывать нельзя!!! Не мы его создали.
            }

            // освобождаем неуправляемые ресурсы
        }

    }
}
