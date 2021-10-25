using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Dto.Order
{
    public class OrderDto : NamedEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
    }
}
