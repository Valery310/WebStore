using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class Order : NamedEntity
    {
        [Required]
        public User User { get; set; }
        [Required]
        [MaxLength(200)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        [NotMapped]
        public decimal TotalPrice => OrderItems?.Sum(i => i.TotalItemPrice) ?? 0m;
    }
}
