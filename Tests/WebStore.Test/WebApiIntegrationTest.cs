using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebStore.Interfaces.Api;
using Microsoft.Extensions.DependencyInjection;

namespace WebStore.Test
{
    [TestClass]
    public class WebApiIntegrationTest
    {
        private readonly string[] _ExpectedValues = Enumerable.Range(1, 10).Select(i => $"TestValue - {i}").ToArray();

        private WebApplicationFactory<WebStore.ServicesHosting.Startup> _Host;

        [TestInitialize]
        public void Initialize()
        {
            var values_service_mock = new Mock<IValuesService>();
            values_service_mock.Setup(s => s.Get()).Returns(_ExpectedValues);

            _Host = new WebApplicationFactory<WebStore.ServicesHosting.Startup>()
                .WithWebHostBuilder(host => host
                .ConfigureServices(service => service
                .AddSingleton(values_service_mock.Object)));
        }

        [TestMethod]
        public async Task GetValues() 
        {
            var client = _Host.CreateClient();
            var response = await client.GetAsync("/WebAPI");
            Assert.Equals(HttpStatusCode.OK, response.StatusCode);

            var parser = new HtmlParser();
            var content_stream = await response.Content.ReadAsStreamAsync();
            var html = parser.ParseDocument(content_stream);

            var item = html.QuerySelectorAll(".container table.table tbody tr td:last-child");
        }
    }
}
