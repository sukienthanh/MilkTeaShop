using Microsoft.AspNetCore.Http;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebClient.Models;
using System.Text.Json.Serialization;

namespace WebClient.ApiClient
{
    public interface IHttpClientCRUD
    {
        public Task<DataResult<T>> PostAsync<T>(T data, string relativeUrl, string token = "", bool requiredAuthorized = true);
        public Task<T2> PostAsync<T1,T2>(T1 data, string relativeUrl, string token = "", bool requiredAuthorized = true);       
        public Task<DataResult<T>> GetAsync<T>(string relativeUrl, string token = "", bool requiredAuthorized = true);
        public Task<DataResult<T>> PutAsync<T>(T data, string relativeUrl, string token = "", bool requiredAuthorized = true);
        public Task<DataResult<T>> DeleteAsync<T>(int[] data, string relativeUrl, string token = "", bool requiredAuthorized = true);
        //public Task<DataResult<bool>> CheckToken(string relativeUrl, string token);
    }
    public class HttpClientCRUDServices:IHttpClientCRUD
    {
        private readonly HttpClient _httpClient = new HttpClient();        
        private readonly ISession _session;
        private readonly JsonSerializerOptions _options;
        public HttpClientCRUDServices(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            //session
            _session = httpContextAccessor.HttpContext.Session;           
            //add base api uri
            _httpClient.BaseAddress = new Uri(config.GetValue<string>("APIBaseURI"));
            //set http request timeout(30s)
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            //clear request header
            _httpClient.DefaultRequestHeaders.Clear();
            //add support json format
            _httpClient.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            // set up the case insensitive deserialization option           
            _options = new JsonSerializerOptions {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                PropertyNameCaseInsensitive = true
            };
        }
        private async void AddTokenToHeader(string token, bool requireAuthorized = true)
        {
            //seset authorization in header
            _httpClient.DefaultRequestHeaders.Clear();
            if (!String.IsNullOrEmpty(token) && requireAuthorized)
            {
                var jwthandler = new JwtSecurityTokenHandler();
                var jwttoken = jwthandler.ReadToken(token);
                var expDate = jwttoken.ValidTo;
                if (expDate < DateTime.UtcNow.AddMinutes(1))
                {
                    var userInfo = _session.Get<UserInfo>("s3cr3cK3y") ?? throw new ArgumentNullException();
                    if (userInfo != null)
                    {
                        var getNewToken = await PostAsync<UserInfo, UserManagerResponse>(userInfo, "Auth/Signup", "", false);
                        if (getNewToken != null && getNewToken.IsSuccess)
                        {
                            token = getNewToken.UserInfo.Token;
                            userInfo.RefreshToken = getNewToken.UserInfo.RefreshToken;
                            userInfo.Token = token;
                            _session.Set<UserInfo>(userInfo);
                        }
                    }
                }                
            }
            
            //add jwt token to request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        //convert data to httpcontent and sent with request 
        private  HttpContent CreateHttpContent<T>(T data)
        {
            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return httpContent;
        }
        public async Task<T2> PostAsync<T1,T2>( T1 data, string relativeUrl, string token = "", bool requiredAuthorized = true)
        {
            AddTokenToHeader(token, requiredAuthorized);
            var response = await _httpClient.PostAsync(relativeUrl, CreateHttpContent<T1>(data));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T2>(content, _options);
            return result;
        }
        public async Task<DataResult<T>> PostAsync<T>(T data, string relativeUrl, string token = "", bool requiredAuthorized = true)
        {
            AddTokenToHeader(token, requiredAuthorized);
            var response = await _httpClient.PostAsync(relativeUrl, CreateHttpContent<T>(data));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DataResult<T>>(content, _options);
            return result;
        }
        public async Task<DataResult<T>> GetAsync<T>(string relativeUrl, string token = "", bool requiredAuthorized = true)
        {
            AddTokenToHeader(token, requiredAuthorized);
            var response = await _httpClient.GetAsync(relativeUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DataResult<T>>(content, _options);
            return result;
        }
        //public async Task<DataResult<bool>> CheckToken(string relativeUrl,string token)
        //{
        //    try
        //    {
        //        AddTokenToHeader(token);
        //        var response = await _httpClient.GetAsync(relativeUrl);
        //        response.EnsureSuccessStatusCode();
        //        var content = await response.Content.ReadAsStringAsync();
        //        var result = JsonSerializer.Deserialize<DataResult<bool>>(content, _options);
        //        return result;
        //    }catch (Exception ex) {
        //        return new DataResult<bool>("unauthorized",false,false);
        //    }
        //}
        public async Task<DataResult<T>> PutAsync<T>(T data, string relativeUrl, string token = "", bool requiredAuthorized = true)
        {
            AddTokenToHeader(token, requiredAuthorized);
            var response = await _httpClient.PutAsync(relativeUrl, CreateHttpContent<T>(data));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DataResult<T>>(content, _options);
            return result;
        }
        public async Task<DataResult<T>> DeleteAsync<T>(int[] data, string relativeUrl, string token = "", bool requiredAuthorized = true)
        {
            AddTokenToHeader(token, requiredAuthorized);
            var a = JsonSerializer.Serialize(data);
            var httpMessage = new HttpRequestMessage(HttpMethod.Delete, relativeUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(httpMessage);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DataResult<T>>(content, _options);
            return result;
        }
    }
}
