using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace WebClient.ApiClient
{
    public interface IAuthClient
    {
        public Task<UserManagerResponse> Login(UserLogin user);
        public Task<UserManagerResponse> Signup(UserRegister user);
        public Task<UserManagerResponse> RefreshToken(UserInfo user);
        //public Task<DataResult<bool>> CheckToken(string token);
    }
    public class AuthClient :IAuthClient
    {
        private readonly IHttpClientCRUD _apiClient;
        public AuthClient(IHttpClientCRUD _apiClient)
        {
            this._apiClient = _apiClient;    
        }
        public async Task<UserManagerResponse> Login(UserLogin user)
        {
            return await _apiClient.PostAsync<UserLogin,UserManagerResponse>(user, "Auth/Login", "", false);
        }
        public async Task<UserManagerResponse> Signup(UserRegister user)
        {
            return await _apiClient.PostAsync<UserRegister,UserManagerResponse>(user, "Auth/Signup", "", false);
        }
        public async Task<UserManagerResponse> RefreshToken(UserInfo user)
        {
            return await _apiClient.PostAsync<UserInfo, UserManagerResponse>(user, "Auth/RefreshToken", user.Token);
        }
        //public async Task<DataResult<bool>> CheckToken(string token)
        //{
        //    return await _apiClient.CheckToken("Auth/CheckToken",token);
        //}
    }

}
