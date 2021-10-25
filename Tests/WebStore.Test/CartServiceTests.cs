using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;
using WebStore.Services.Implementations;
using WebStore.Services.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace WebStore.Test
{
    [TestClass]
    public class CartServiceTests
    {
        [Fact]
        public void Cart_Class_ItemsCount_Returns_Correct_Quantity()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem()
                    {
                        ProductId = 1,
                        Quantity = 1
                    },
                    new CartItem()
                    {
                        ProductId = 3,
                        Quantity = 3
                    }
                }
            };

            var result = cart.ItemsCount;
            Assert.Equal(4, result);
        }

        [Fact]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            var cartViewModel = new CartViewModel()
            {
                Items = new Dictionary<ProductViewModel, int>()
                {
                    {
                        new ProductViewModel()
                        {
                            Id = 1,
                            Name = "TestItem",
                            Price = 5.0m
                        },
                        1
                    },
                    {
                        new ProductViewModel()
                        {
                            Id = 2,
                            Name = "TestItem2",
                            Price = 1.0m
                        },
                        2
                    },
                }
            };

            var result = cartViewModel.ItemsCount;
            Assert.Equal(3, result);
        }

        [Fact]
        public void CartService_AddToCart_WorksCorrect()
        {          
            var cart = new Cart()
            {
                Items = new List<CartItem>()
            };

            var productData = new Mock<IProductData>();
            productData.Setup(c => c.GetProducts(It.IsAny<ProductFilter>())).ReturnsAsync(new[] { new Product { Id = 1 }, new Product { Id = 2 }, new Product { Id = 3 } } );

            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(cartStore.Object, productData.Object);

            cartService.AddToCart(5);
            Assert.Equal(1, cart.ItemsCount);
            Assert.Single(cart.Items);
        //    Assert.Equal(1, cart.Items.Count);
            Assert.Equal(5, cart.Items[0].ProductId);
        }

        [Fact]
        public void CartService_AddToCart_Increment_Quantity()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem(){ProductId = 5, Quantity = 2}
                }
            };
            var productData = new Mock<IProductData>();
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);
            var cartService = new CartService(cartStore.Object, productData.Object);
            cartService.AddToCart(5);
            Assert.Equal(1, cart.Items.Count);
            Assert.Single(cart.Items);
            Assert.Equal(3, cart.ItemsCount);
        }

        [Fact]
        public void CartService_RemoveFromCart_Removes_Correct_Item()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem(){ProductId = 1,Quantity = 3},
                    new CartItem(){ProductId = 2, Quantity = 1}
                }
            };
            var productData = new Mock<IProductData>();
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);
            var cartService = new CartService(cartStore.Object, productData.Object);
            cartService.RemoveFromCart(1);
            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(2, cart.Items[0].ProductId);
        }

        [Fact]
        public void CartService_RemoveAll_Clear_Cart()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem(){ProductId = 1,Quantity = 3},
                    new CartItem(){ProductId = 2, Quantity = 1}
                }
            };
            var productData = new Mock<IProductData>();
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);
            var cartService = new CartService(cartStore.Object, productData.Object);
            cartService.RemoveAll();
            Assert.Equal(0, cart.Items.Count);
        }

        [Fact]
        public void CartService_Decrement_Correct()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem(){ProductId = 1,Quantity = 3},
                    new CartItem(){ProductId = 2, Quantity = 1}
                }
            };
            var productData = new Mock<IProductData>();
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);
            var cartService = new CartService(cartStore.Object, productData.Object);
            cartService.DecrementFromCart(1);
            Assert.Equal(3, cart.ItemsCount);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(1, cart.Items[0].ProductId);
        }

        [Fact]
        public void CartService_Remove_Item_When_Decrement()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem(){ProductId = 1,Quantity = 3},
                    new CartItem(){ProductId = 2, Quantity = 1}
                }
            };

            var productData = new Mock<IProductData>();
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);
            var cartService = new CartService(cartStore.Object, productData.Object);
            cartService.DecrementFromCart(2);
            Assert.Equal(3, cart.ItemsCount);
            Assert.Equal(1, cart.Items.Count);
        }

        [Fact]
        public async void CartService_TransformCart_WorksCorrect()
        {
            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem(){ProductId = 1, Quantity = 4}
                }
            };
            var products = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = 1,
                    ImageUrl = "",
                    Name = "Test",
                    Order = 0,
                    Price = 1.11m,
                }
            };
            var productData = new Mock<IProductData>();
            productData.Setup(c => c.GetProducts(It.IsAny<ProductFilter>()).Result).Returns(products.FromDTO());
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);
            var cartService = new CartService(cartStore.Object, productData.Object);
            var result = await cartService.TransformCart();
            Assert.Equal(4, result.ItemsCount);
            Assert.Equal(1.11m, result.Items.First().Key.Price);
        }
    }

}
