using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;
using Xunit;

namespace WebStore.Test
{
    [TestClass]
    public class CatalogControllerTests
    {
        [Fact]
        public async void ProductDetails_Returns_View_With_Correct_Item()
        {
            // Arrange
            var productMock = new Mock<IProductData>();

            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("3");

            productMock.Setup(p => p.GetProductById(It.IsAny<int>())).ReturnsAsync(new Product()
            {
                Id = 1,
                Name = "Test",
                ImageUrl = "TestImage.jpg",
                Order = 0,
                Price = 10,
                Brand = new Brand()
                {
                    Id = 1,
                    Name = "TestBrand"
                }
            });

            var controller = new CatalogController(productMock.Object, configuration.Object);
            // Act
            var result = await controller.ProductDetails(1);
            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
            Xunit.Assert.Equal(1, model.Id);
            Xunit.Assert.Equal("Test", model.Name);
            Xunit.Assert.Equal(10, model.Price);
            Xunit.Assert.Equal("TestBrand", model.Brand.Name);
        }

        [Fact]
        public async void ProductDetails_Returns_NotFound()
        {
            // Arrange
            var productMock = new Mock<IProductData>();
            productMock.Setup(p => p.GetProductById(It.IsAny<int>())).ReturnsAsync((Product)null);

            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("3");

            var controller = new CatalogController(productMock.Object, configuration.Object);
            // Act
            var result = await controller.ProductDetails(1);
            // Assert
            Xunit.Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Shop_Method_Returns_Correct_View()
        {
            // Arrange
            var productMock = new Mock<IProductData>();
            productMock.Setup(p => p.GetProducts(It.IsAny<ProductFilter>())).ReturnsAsync(new PageProduct() 
            {
                Products = new List<Product>()
             {
                new Product()
                {
                    Id = 1,
                    Name = "Test",
                    ImageUrl = "TestImage.jpg",
                    Order = 0,
                    Price = 10,
                    Brand = new Brand()
                    {
                        Id = 1,
                        Name = "TestBrand"
                    }
                },
                new Product()
                {
                    Id = 2,
                    Name = "Test2",
                    ImageUrl = "TestImage2.jpg",
                    Order = 1,
                    Price = 22,
                    Brand = new Brand()
                    {
                        Id = 1,
                        Name = "TestBrand"
                    }
                }
              }
                , TotalCount = 2
            }
         
           );

            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("3");

            var controller = new CatalogController(productMock.Object, configuration.Object);
            // Act
            var result = await controller.Shop(1, 5);
            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<CatalogViewModel>(viewResult.ViewData.Model);
            Xunit.Assert.Equal(2, model.Products.Count());
            Xunit.Assert.Equal(5, model.BrandId);
            Xunit.Assert.Equal(1, model.SectionId);
            Xunit.Assert.Equal("TestImage2.jpg", model.Products.ToList()[1].ImageUrl);
        }
    }
}
