using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<ProductViewModel, int> _Items = new Dictionary<ProductViewModel, int>();
            _Items.Add(new ProductViewModel(), _cartService.GetItemsCoumtFromCart());

            return View(new CartViewModel()
            {          
                Items = _Items
            }); ; 
        }
    }
}
