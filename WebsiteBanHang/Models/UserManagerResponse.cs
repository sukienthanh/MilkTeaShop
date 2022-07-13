using Microsoft.AspNetCore.Identity;

namespace MilkTeaShop.Models
{
    public class UserManagerResponse
    {
        public string? Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public UserInfo? UserInfo { get; set; }        
        public UserManagerResponse() { }
        public UserManagerResponse(string? message, 
            bool status, 
            IEnumerable<string>? error = null,
            UserInfo? userInfo = null
            )
        {
            Message = message; 
            Errors = error;
            IsSuccess = status;
            UserInfo = userInfo;            
        }
    }
}
