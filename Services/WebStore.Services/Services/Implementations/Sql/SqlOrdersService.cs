using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Dto;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Implementations.Sql
{
    public class SqlOrdersService : IOrdersService
    {
        private readonly WebStoreContext _context;
        private readonly ILogger<SqlOrdersService> _logger;
        private readonly UserManager<User> _userManager;

        public SqlOrdersService(WebStoreContext context, UserManager<User> userManager, ILogger<SqlOrdersService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userName)
        {
            return await _context.Orders
                .Include("User")
                .Include("OrderItems")
                .Where(o => o.User.UserName.Equals(userName))
                .Select(o => new OrderDto()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address,
                    Date = o.Date,
                    Phone = o.Phone,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto()
                    {
                        Id = oi.Id,
                        Price = oi.Price,
                        Quantity = oi.Quantity
                    }).ToArray()
                })
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.Include("OrderItems").FirstOrDefaultAsync(o => o.Id.Equals(id)).ConfigureAwait(false);

            if (order == null)
            {
                return null;
            }

            return new OrderDto()
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Date = order.Date,
                Phone = order.Phone,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto()
                {
                    Id = oi.Id,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToArray()
            };
        }

        public async Task<OrderDto> CreateOrderAsync(OrderViewModel OrderModel, CartViewModel Cart, string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} отсутствует в БД");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var order = new OrderDto
            {
                User = user,
                Address = OrderModel.Address,
                Phone = OrderModel.Phone,
                Name = OrderModel.Name,
            };
            var product_ids = Cart.Items.Select(item => item.Key.Id).ToArray();

            var cart_products = await _context.Products
               .Where(p => product_ids.Contains(p.Id))
               .ToArrayAsync();

            order.OrderItems = Cart.Items.Join(
                cart_products,
                cart_item => cart_item.Key.Id,
                cart_product => cart_product.Id,
                (cart_item, cart_product) => new OrderItemDto
                {
                    Order = order,
                    Product = cart_product,
                    Price = cart_product.Price, // здесь можно применить скидки...
                    Quantity = cart_item.Value,
                }).ToArray();

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Заказ для пользователя {0} сформирован с id:{1}", UserName, order.Id);

            await transaction.CommitAsync();

            return new OrderDto() { Id = order.Id, 
                Address = order.Address, 
                Date = order.Date, 
                Description = order.Description, 
                Name = order.Name, 
                OrderItems = order.OrderItems,
                Phone = order.Phone, 
                User = order.User };
        }


        //public async Task<OrderDto> CreateOrderAsync(CreateOrderModel orderModel, string userName)
        //{
        //    var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        var order = new Order()
        //        {
        //            Address = orderModel.OrderViewModel.Address,
        //            Name = orderModel.OrderViewModel.Name,
        //            Date = DateTime.Now,
        //            Phone = orderModel.OrderViewModel.Phone,
        //            User = user
        //        };
        //        _context.Orders.Add(order);

        //        foreach (var item in orderModel.OrderItems)
        //        {
        //            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(item.Id));

        //            if (product == null)
        //            {
        //                throw new InvalidOperationException("Продукт не найден в базе");
        //            }
        //            var orderItem = new OrderItem()
        //            {
        //                Order = order,
        //                Price = product.Price,
        //                Quantity = item.Quantity,
        //                Product = product
        //            };

        //            _context.OrderItems.Add(orderItem);
        //        }
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return await GetOrderByIdAsync(order.Id);
        //    }
        //}
    }
}