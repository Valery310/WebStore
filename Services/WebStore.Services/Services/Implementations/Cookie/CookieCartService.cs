using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Interfaces.Services;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;
using WebStore.Domain.Filters;
using System.Threading.Tasks;

namespace WebStore.Services.Implementations
{
    public class CookieCartService : ICartService
    {
        private readonly IProductData _productData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName;

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;

                var cart_cookie = context.Request.Cookies[_cartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCookies(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
            
        }

        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cookie);
        }

        public CookieCartService(IProductData productData, IHttpContextAccessor httpContextAccessor)
        {
            _productData = productData;
            _httpContextAccessor = httpContextAccessor;
            var user_name = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? _httpContextAccessor.HttpContext.User.Identity.Name : null;

            _cartName = $"WebStore.Cart{user_name}";
        }


        public void AddToCart(int id)
        {
            var cart = Cart;
            if (cart.Items == null)
            {
                cart.Items = new List<CartItem>();
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            }
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
            }               
            else
            {
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            }
               
            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                if (item.Quantity > 0)
                    item.Quantity--;
                if (item.Quantity == 0)
                    cart.Items.Remove(item);
            }
            Cart = cart;
        }


        public void RemoveAll()
        {
            Cart = new Cart { Items = new List<CartItem>() };
        }


        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                cart.Items.Remove(item);
            }
            Cart = cart;
        }

        public async Task<CartViewModel> TransformCart()
        {

            var products = await _productData.GetProducts(new ProductFilter()
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
            }).ConfigureAwait(false);
            
            var productsVM = products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
            }).ToList();

            var r = new CartViewModel
            {
                Items = Cart.Items.ToDictionary(x => productsVM.First(y => y.Id ==
                x.ProductId), x => x.Quantity)
            };
            return r;
        }
    }
}
