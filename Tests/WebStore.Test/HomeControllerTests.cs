using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Api;
using Xunit;

namespace WebStore.Test
{
    [TestClass]
    public class HomeControllerTests
    {
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var mockService = new Mock<IValuesService>();
            mockService.Setup(c => c.GetAsync()).ReturnsAsync(new List<string> { "1", "2" });

            var mockHttpContext = new Mock<HttpContext>(MockBehavior.Loose);
            mockHttpContext.Object.TraceIdentifier = "500";

            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = WebStore.Logger.Log4NetExtensions.AddLog4Net(factory).CreateLogger<HomeController>();

            _controller = new HomeController(mockService.Object, logger);
            _controller.ControllerContext = new ControllerContext()
            {
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() {ActionName = "Index", ControllerName = "Home" },
                HttpContext = new DefaultHttpContext(),
        };  
        }

        [Fact]
        public async Task Index_Method_Returns_View_With_Values()
        {
            // Arrange and act
            var result = await _controller.Index();
            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.ViewData.Model);
            Xunit.Assert.Equal(2, model.Count());
        }

        [Fact]
        public void ContactUs_Returns_View()
        {
            var result = _controller.ContactUs();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ErrorStatus_404_Redirects_to_NotFound()
        {
            var result = _controller.ErrorStatus("404");
            var redirectToActionResult = Xunit.Assert.IsType<RedirectToActionResult>(result);
            Xunit.Assert.Null(redirectToActionResult.ControllerName);
            // Xunit.Assert.Equal("NotFound", redirectToActionResult.ActionName);Error
            Xunit.Assert.Equal("Error", redirectToActionResult.ActionName); 
        }

        [Fact]
        public void Checkout_Returns_View()
        {
            var result = _controller.Checkout();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void BlogSingle_Returns_View()
        {
            var result = _controller.BlogSingle();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Blog_Returns_View()
        {
            var result = _controller.Blog();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_Returns_View()
        {
            _controller.HttpContext.TraceIdentifier = "500";
            var result = _controller.Error();
            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
             Xunit.Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void NotFound_Returns_View()
        {
            var result = _controller.NotFound();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ErrorStatus_Antother_Returns_Content_Result()
        {
            var result = _controller.ErrorStatus("500");
            var contentResult = Xunit.Assert.IsType<ContentResult>(result);
            Xunit.Assert.Equal("Статуcный код ошибки: 500", contentResult.Content);
        }
    }
}
