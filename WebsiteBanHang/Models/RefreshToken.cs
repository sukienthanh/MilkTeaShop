using System;
using System.Collections.Generic;

namespace MilkTeaShop.Models
{
    public partial class RefreshToken
    {
        public RefreshToken()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Token { get; set; }
        public bool? IsRevoked { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
