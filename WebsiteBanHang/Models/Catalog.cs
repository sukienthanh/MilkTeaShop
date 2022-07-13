using System;
using System.Collections.Generic;

namespace MilkTeaShop.Models
{
    public partial class Catalog
    {
        public Catalog()
        {
            Products = new HashSet<Product>();
        }

        public int CataId { get; set; }
        public string? CataName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
