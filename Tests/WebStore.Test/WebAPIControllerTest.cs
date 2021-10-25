using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces.Api;
using WebStore.ServicesHosting.Controllers;
using Xunit;
using Assert = Xunit.Assert;

namespace WebStore.Test
{
    [TestClass]
    public class WebAPIControllerTest
    {
        [Fact]
        public void Index_returns_with_Data_Value() 
        {
            var Temp_Data = Enumerable.Range(1,10).Select(i => $"Value-{i}").ToArray();
            var values_service = new Mock<IValuesService>();
            values_service.Setup(c => c.Get()).Returns(Temp_Data);
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = WebStore.Logger.Log4NetExtensions.AddLog4Net(factory).CreateLogger<ValuesApiController>();
            var controller = new ValuesApiController(logger);

            var result = controller.Get();

            var view_result = Assert.IsType<OkObjectResult>(result);

            var _model = view_result.Value;

            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Value);

            var i = 0;

            foreach (var actual in model)
            {
                var expected_value = Temp_Data[i++];
                Assert.Equal(expected_value, actual);
            }

         //   values_service.Verify(s => s.Get(), Times.AtLeastOnce()) ;
         //   values_service.VerifyNoOtherCalls();

        }
    }
}
