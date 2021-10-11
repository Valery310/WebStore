using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        protected HttpClient Client;
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        protected abstract string ServiceAddress { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация проекта</param>
        protected BaseClient(IConfiguration configuration)
        {
            // Создаем экземпляр клиента
            Client = new HttpClient
            {
                // Базовый адрес, на котором будут хостится сервисы
                BaseAddress = new Uri(configuration["ClientAdress"])
            };
            Client.DefaultRequestHeaders.Accept.Clear();

            // Устанавливаем хедер, который будет говорит серверу, чтобы он отправлял данные в формате json
        Client.DefaultRequestHeaders.Accept.Add(new
        MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
