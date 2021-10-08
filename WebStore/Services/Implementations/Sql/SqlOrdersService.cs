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
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o =>o.Product)
                .Where(o => o.User.UserName == userName)
                .ToArrayAsync()
                .ConfigureAwait(false);
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
               .Include(o => o.User)
               .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .FirstOrDefaultAsync(o => o.Id == id)
               .ConfigureAwait(false);
        }
        public async Task<Order> CreateOrderAsync(OrderViewModel orderModel, CartViewModel transformCart, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if (user is null)
            {
                throw new InvalidOperationException($"Пользователь {userName} не найден");
            }

            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var order = new Order()
                {
                    Address = orderModel.Address,
                    Phone = orderModel.Phone,
                    User = user,
                    Description = orderModel.Description
                };

                var products_ids = transformCart.Items.Select(Items => Items.Key.Product.Id).ToArray();

                var cart_products = await _context.Products.Where(p => products_ids.Contains(p.Id)).ToArrayAsync();

                order.OrderItems = transformCart.Items.Join(
                    cart_products,
                    cart_item => cart_item.Key.Id,
                    cart_product => cart_product.Id,
                    (cart_item, cart_product) => new OrderItem
                    {
                        Order = order,
                        Product = cart_product,
                        Price = cart_product.Price, //тут можно добавить скидку
                        Quantity = cart_item.Key.Quantity
                    }
                    ).ToArray();

                await _context.Orders.AddAsync(order);

                //foreach (var item in transformCart.Items)
                //{
                //    var productVm = item.Key;
                //    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(productVm.Id));
                //    if (product == null)
                //        throw new InvalidOperationException("Продукт не найден в базе");
                //        var orderItem = new OrderItem()
                //        {
                //            Order = order,
                //            Price = product.Price,
                //            Quantity = item.Value,
                //            Product = product
                //        };

                //    await _context.OrderItems.AddRangeAsync(order.OrderItems); нет неообходимости, так как контекст сам определит, что в order есть и orderitems и отправит в БД.
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
        }
    }
}
}