using System;
using System.Collections.Generic;

namespace MilkTeaShop.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public virtual Order? Order { get; set; }
        public virtual User? User { get; set; }
    }
}
