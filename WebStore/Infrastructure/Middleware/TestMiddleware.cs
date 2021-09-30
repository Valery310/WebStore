using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<TestMiddleware> _Logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> Logger)
        {
            _Next = next;
            _Logger = Logger;
        }
    
    public async Task InvokeAsync(HttpContext context)
        {
            //context.Response.WriteAsJsonAsyncc(); Следующй делегат вызывать не надо, если уже тправлен ответ клиенту на запрос
            // await _Next();
            //context.Response.WriteAsJsonAsyncc(); Но можно запустить, что-то уже после.
            //Педобработка
            var processing = _Next(context);
            //Обработка параллельно
            await processing;

            //Постобработка
        }
    }


}
