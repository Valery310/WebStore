using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.Implementations
{
    public class CookieCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string _CartName;

        public Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;

                var cart_cookie = context.Request.Cookies[_CartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart() { Items = new System.Collections.Generic.List<CartItem>() };
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart), new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(1)
                    });
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookie, new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCookies(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
        }

        private void ReplaceCookies(IResponseCookies cookies, string cookie, CookieOptions cookieOptions = null)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie, cookieOptions);
        }

        public CookieCartStore(IHttpContextAccessor HttpContextAccessor)
        {
            _HttpContextAccessor = HttpContextAccessor;

            var user = HttpContextAccessor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{user_name}";
        }
    }
}
