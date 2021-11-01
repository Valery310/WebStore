using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _CartStore;
        private readonly IProductData _ProductData;

        public CartService(ICartStore CartStore, IProductData ProductData)
        {
            _CartStore = CartStore;
            _ProductData = ProductData;
        }

        public void AddToCart(int Id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null)
                cart.Items.Add(new CartItem { ProductId = Id, Quantity = 1 });
            else
                item.Quantity++;

            _CartStore.Cart = cart;
        }

        public void DecrementFromCart(int Id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public int GetItemsCoumtFromCart()
        {
            var cart = _CartStore.Cart;
            return cart.ItemsCount;          
        }

        public Cart GetItemsFromCart()
        {
            var cart = _CartStore.Cart;
            return cart;
        }

        public void RemoveFromCart(int Id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null) return;

            cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public void RemoveAll()
        {
            //_CartStore.Cart = new();

            var cart = _CartStore.Cart;
            cart.Items.Clear();
            _CartStore.Cart = cart;
        }

        public async Task<CartViewModel> TransformCart()
        {
            var r = new CartViewModel();

            if (_CartStore.Cart.ItemsCount >= 0)
            {
                var products = (await _ProductData.GetProducts(new ProductFilter()
                {
                    Ids = _CartStore.Cart.Items.Select(i => i.ProductId).ToArray()
                })
           .ConfigureAwait(false));

                var tenp = products.Products.Select(p => new ProductViewModel()
               {
                   Id = p.Id,
                   ImageUrl = p.ImageUrl,
                   Name = p.Name,
                   Order = p.Order,
                   Price = p.Price,
               }).ToList();

                //var products = (await _ProductData.GetProducts(new ProductFilter()
                //{
                //    Ids = _CartStore.Cart.Items.Select(i => i.ProductId).ToArray()
                //})
                //.ConfigureAwait(false)).Products
                //.Select(p => new ProductViewModel()
                //{
                //    Id = p.Id,
                //    ImageUrl = p.ImageUrl,
                //    Name = p.Name,
                //    Order = p.Order,
                //    Price = p.Price,
                //}).ToList();

                //var productsVM = products.Select(p => new ProductViewModel()
                //{
                //    Id = p.Id,
                //    ImageUrl = p.ImageUrl,
                //    Name = p.Name,
                //    Order = p.Order,
                //    Price = p.Price,
                //}).ToList();

                //r = new CartViewModel
                //{
                //    Items = _CartStore.Cart.Items.ToDictionary(x => products.First(y => y.Id ==
                //    x.ProductId), x => x.Quantity)
                //};
                r = new CartViewModel
                {
                    Items = _CartStore.Cart.Items.ToDictionary(x => tenp.First(y => y.Id ==
                    x.ProductId), x => x.Quantity)
                };
            }
            else
            {
                r = new CartViewModel
                {
                    Items = new Dictionary<ProductViewModel, int>()

                };
            }

            return r;
        }
    }
}
