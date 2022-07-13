using System.ComponentModel.DataAnnotations;

namespace MilkTeaShop.Models
{
    public class UserLogin
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50), MinLength(5)]
        [DataType(DataType.Password)]        
        public string Password { get; set; }       
    }
    public class UserRegister
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(50), MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(50), MinLength(5)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

    }
    public class UserInfo
    {
        public string? Username { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public string? Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}
