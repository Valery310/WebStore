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

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName)
        {
            return await _context.Orders
           .Include(order => order.User)
           .Include(order => order.OrderItems)
           .ThenInclude(item => item.Product)
           .Where(order => order.User.UserName == userName)
           .ToArrayAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.Include("OrderItems").FirstOrDefaultAsync(o => o.Id.Equals(id)).ConfigureAwait(false);

            if (order == null)
            {
                return null;
            }

            return new Order()
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Date = order.Date,
                Phone = order.Phone,
                OrderItems = order.OrderItems
            };
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto orderModel, string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user is null)
            {
                _logger.LogError("Пользователь {0} отсутствует в БД", UserName);
                throw new InvalidOperationException($"Пользователь {UserName} отсутствует в БД");
            }

            using (_logger.BeginScope("Создание заказа от {0}", UserName))
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var order = new Order
                {
                    User = user,
                    Address = orderModel.OrderModel.Address,
                    Phone = orderModel.OrderModel.Phone,
                    Name = orderModel.OrderModel.Name,
                    Description = orderModel.OrderModel.Description
                };
                var product_ids = orderModel.Items.Select(item => item.Id).ToArray();

                var cart_products = await _context.Products
                   .Where(p => product_ids.Contains(p.Id))
                   .ToArrayAsync();

                order.OrderItems = orderModel.Items.Join(
                    cart_products,
                    cart_item => cart_item.Id,
                    cart_product => cart_product.Id,
                    (cart_item, cart_product) => new OrderItem
                    {
                        Order = order,
                        Product = cart_product,
                        Price = cart_product.Price, // здесь можно применить скидки...
                    Quantity = cart_item.Quantity,
                    }).ToArray();

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Заказ для пользователя {0} сформирован с id:{1}", UserName, order.Id);

                await transaction.CommitAsync();

                return new Order()
                {
                    Id = order.Id,
                    Address = order.Address,
                    Date = order.Date,
                    Description = order.Description,
                    Name = order.Name,
                    OrderItems = order.OrderItems,
                    Phone = order.Phone,
                    User = order.User
                };
            }      
        }
    }
}