using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.ViewModel
{
    public class UserOrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal TotalSum { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
    }
}
