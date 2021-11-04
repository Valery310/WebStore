using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Test
{
    [TestClass]
    public class WebAPIIntegrationTest
    {
        private readonly string[] _ExpectedValues = Enumerable.Range(1, 10).Select(i => $"TestValue - {i}").ToArray();

        private WebApplicationFactory<Startup> _Host;

        [TestInitialize]
        public void Initialize()
        {
            var values_service_mock = new Mock<IValuesService>();
            values_service_mock.Setup(s => s.Get()).Returns(_ExpectedValues);

            var cart_service_mock = new Mock<ICartService>();
            cart_service_mock.Setup(c => c.TransformCart()).ReturnsAsync(() => new CartViewModel { Items = (System.Collections.Generic.Dictionary<ProductViewModel, int>)Enumerable.Empty<(ProductViewModel, int)>() });

            _Host = new WebApplicationFactory<Startup>()
               .WithWebHostBuilder(host => host
                   .ConfigureServices(services => services
                       .AddSingleton(values_service_mock.Object)
                       .AddSingleton(cart_service_mock.Object)));
        }

        [Xunit.Fact, Timeout(3000)]
        public async Task GetValues()
        {
            Initialize();
            var client = _Host.CreateClient();

            var response = await client.GetAsync("/WebAPI");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var parser = new HtmlParser();

            var content_stream = await response.Content.ReadAsStreamAsync();
            var html = parser.ParseDocument(content_stream);

            var items = html.QuerySelectorAll(".container table.table tbody tr td:last-child");

            var actual_values = items.Select(item => item.Text());

            Assert.Equal(_ExpectedValues, actual_values);
        }
    }
}
