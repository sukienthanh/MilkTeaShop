using System;
using System.Collections.Generic;

namespace MilkTeaShop.Models
{
    public partial class User
    {
        public User()
        {
            Payments = new HashSet<Payment>();
        }

        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ReFreshToken { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
