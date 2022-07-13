using System;
using System.Collections.Generic;

namespace MilkTeaShop.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? Note { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
