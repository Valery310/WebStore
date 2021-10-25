using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace WebStore.Test
{
    [TestClass]
    public class CartControllerTests
    {
        [Fact]
        public async void CheckOut_ModelState_Invalid_Returns_ViewModel()
        {
            var mockCartService = new Mock<ICartService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var controller = new CartController(mockCartService.Object, mockOrdersService.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            var result = await controller.CheckOut(new OrderViewModel()
            {
                Name = "test"
            }, mockOrdersService.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.ViewData.Model);
            Assert.Equal("test", model.Order.Name);
        }

        [Fact]
        public async void CheckOut_Calls_Service_And_Return_Redirect()
        {
            #region Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));
            // setting up cartService
            var mockCartService = new Mock<ICartService>();
            mockCartService.Setup(c => c.TransformCart()).ReturnsAsync(new CartViewModel()
            {
                Items = new Dictionary<ProductViewModel, int>()
                {
                    { 
                        new ProductViewModel(),
                        1 
                    }
                }
            });

            // setting up ordersService
            var mockOrdersService = new Mock<IOrdersService>();
            mockOrdersService.Setup(c => c.CreateOrderAsync(It.IsAny<CreateOrderDto>(), It.IsAny<string>())).ReturnsAsync(new Order() { Id = 1 });

            var controller = new CartController(mockCartService.Object, mockOrdersService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = user
                    }
                }
            };
            #endregion

            // Act
            var result = await controller.CheckOut(new OrderViewModel()
            {
                Name = "test",
                Address = "",
                Phone = ""
            }, mockOrdersService.Object);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("OrderConfirmed", redirectResult.ActionName);
            Assert.Equal(1, redirectResult.RouteValues["id"]);
        }
    }
}
