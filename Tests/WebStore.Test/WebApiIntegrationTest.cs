using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebStore.Interfaces.Api;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Assert = Xunit.Assert;

namespace WebStore.Test
{
    [TestClass]
    public class WebApiIntegrationTest
    {
        private readonly string[] _ExpectedValues = Enumerable.Range(1, 10).Select(i => $"TestValue - {i}").ToArray();

        private WebApplicationFactory<WebStore.ServicesHosting.Startup> _Host;

        public void Initialize()
        {
            var values_service_mock = new Mock<IValuesService>();
            values_service_mock.Setup(s => s.Get()).Returns(_ExpectedValues);

            _Host = new WebApplicationFactory<WebStore.ServicesHosting.Startup>()
                .WithWebHostBuilder(host => host
                .ConfigureServices(service => service
                .AddSingleton(values_service_mock.Object)));
        }

        [Fact]
        public async Task GetValues() 
        {
            Initialize();

            var client = _Host.CreateClient();
            // var response = await client.GetAsync("/WebAPI");
            var response = await client.GetAsync("/swagger/index");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var parser = new HtmlParser();
            var content_stream = await response.Content.ReadAsStreamAsync();
            var html = parser.ParseDocument(content_stream);

            var item = html.QuerySelectorAll(".container table.table tbody tr td:last-child");
            var actual_values = item.Select(i => i.TextContent);
            Assert.Equal(_ExpectedValues, actual_values);
        }
    }
}
