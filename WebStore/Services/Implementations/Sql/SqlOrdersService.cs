using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.ViewModel;

namespace WebStore.Services.Implementations.Sql
{
    public class SqlOrdersService : IOrdersService
    {
        private readonly WebStoreContext _context;
        private readonly UserManager<User> _userManager;
        public SqlOrdersService(WebStoreContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IEnumerable<Order> GetUserOrders(string userName)
        {
            return _context.Orders.Include("User").Include("OrderItems").Where(o
            => o.User.UserName.Equals(userName)).ToList();
        }
        public Order GetOrderById(int id)
        {
            return _context.Orders.Include("OrderItems").FirstOrDefault(o =>
            o.Id.Equals(id));
        }
        public async Task<Order> CreateOrderAsync(OrderViewModel orderModel, CartViewModel transformCart, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var order = new Order()
                {
                    Address = orderModel.Address,
                    Name = orderModel.Name,
                    Date = DateTime.Now,
                    Phone = orderModel.Phone,
                    User = user
                };
                _context.Orders.Add(order);
                foreach (var item in transformCart.Items)
                {
                    var productVm = item.Key;
                    var product = await _context.Products.FirstOrDefaultAsync(p =>
                    p.Id.Equals(productVm.Id));
                    if (product == null)
                        throw new InvalidOperationException("Продукт не найден в базе");
                        var orderItem = new OrderItem()
                        {
                            Order = order,
                            Price = product.Price,
                            Quantity = item.Value,
                            Product = product
                        };
             
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return order;
        }
    }
}
}