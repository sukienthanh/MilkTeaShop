using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MilkTeaShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public double? ShippingCost { get; set; }
        public double? TotalAmount { get; set; }
        public double? DiscountAmount { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public string? OrderNote { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        //[JsonIgnore]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
