using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;

namespace WebStore.Domain.ViewModel
{
    public class CartViewModel
    {
      //  public Dictionary<OrderItem, int> Items { get; set; }
        public Dictionary<ProductViewModel, int> Items { get; set; }
        public int ItemsCount
        {
            get
            {
                return Items?.Sum(x => x.Value) ?? 0;
            }
        }
    }
}
