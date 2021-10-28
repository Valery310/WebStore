using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces;

namespace WebStore.Test
{
    // https://docs.microsoft.com/ru-ru/aspnet/core/test/integration-tests?view=aspnetcore-5.0
    [TestClass]
    public class ValuesControllerTest
    {

        private readonly WebApplicationFactory<Startup> _Host = new WebApplicationFactory<Startup>();

        [TestMethod]
        public async Task GetValues_IntegrityTest()
        {
            var client = _Host.CreateClient();

            var response = await client.GetAsync(WebAPIAddresses.Values);

            response.EnsureSuccessStatusCode();

            var values = response.Content.ReadFromJsonAsync<IEnumerable<string>>();

            //
            //Парсер для html.
            // https://habr.com/ru/post/273807/
            //var parser = new HtmlParser();
            //var html = await parser.ParseDocumentAsync(await response.Content.ReadAsStreamAsync());
        }
    }
}
