using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface IAuth
    {
        public string GenerateToken(UserInfo user);
        public Task<UserManagerResponse> Login(UserLogin user);
        public Task<UserManagerResponse> Register(UserRegister user);
        public Task<UserManagerResponse> RefreshToken(UserInfo userInfo);
    }      
}
