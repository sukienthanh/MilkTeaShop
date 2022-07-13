using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MilkTeaShop.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? CatalogId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        //[JsonIgnore]
        public virtual Catalog? Catalog { get; set; }
        //[JsonIgnore]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
