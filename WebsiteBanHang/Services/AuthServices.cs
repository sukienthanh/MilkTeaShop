using MilkTeaShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using MilkTeaShop.Helper;

namespace MilkTeaShop.Services
{
    public class AuthServices : IAuth
    {
        private readonly IConfiguration _config;
        private readonly IUser _userService;        
        public AuthServices(IConfiguration _config, IUser _userService)
        {           
            this._config = _config;
            this._userService = _userService;
        }
        public async Task<UserManagerResponse> Register(UserRegister data)
        {
            if (data != null)
            {
                if (data.Password != data.ConfirmPassword)
                    return new UserManagerResponse("Confirm password isn't match", false);
                var check = await _userService.Get(null, "", data.Email);
                if (check != null && check.Status)
                    return new UserManagerResponse("That email has already used by another user", false);
                check = await _userService.Get(null, data.Username, "");
                if (check != null && check.Status)
                    return new UserManagerResponse("That username has already used by another user", false);
                var user = new List<User>();
                user.Add(new User
                {
                    UserName = data.Username,
                    Email = data.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
                });

                var response = await _userService.Add(user);

                if (response != null)
                {
                    if (response.Status && response.Data != null)
                    {
                        return await Login(new UserLogin()
                        {                            
                            Username = data.Username,
                            Password = data.Password
                        });
                    }                    
                     return new UserManagerResponse(response.Message, response.Status);
                }
                return new UserManagerResponse("Add new user service return response with null value", false);
            }
            return new UserManagerResponse("data null", false);
        }

        public async Task<UserManagerResponse> Login(UserLogin data)
        {
            if (data != null)
            {
                var getUser = await _userService.Get(null, data.Username);
                if (getUser != null && getUser.Data != null)
                {
                    var checkPasswd = BCrypt.Net.BCrypt.Verify(data.Password, getUser.Data.Password);
                    if (checkPasswd)
                    {                        
                        var refreshToken = GenerateRefreshToken();
                        var userInfo = new UserInfo
                        {
                            Username = getUser.Data.UserName,
                            Email = getUser.Data.Email,
                            RefreshToken = refreshToken                           
                        };
                        userInfo.Role = getUser.Data.Role != null ? getUser.Data.Role.RoleName : userInfo.Role;
                        userInfo.Token = GenerateToken(userInfo);
                        getUser.Data.ReFreshToken = refreshToken;
                        getUser.Data.ExpiryDate = DateTime.Now.AddMonths(1);
                        var updateData = new List<User>();
                        updateData.Add(getUser.Data);
                        await _userService.Update(updateData);

                        return new UserManagerResponse("Login successfully", true, null, userInfo);
                    }
                    return new UserManagerResponse("Wrong password", false);
                }
                return new UserManagerResponse("Wrong username", false);
            }
            return new UserManagerResponse("data null", false);
        }

        public string GenerateToken(UserInfo user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserManagerResponse> RefreshToken(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                var getUser = await _userService.Get(null, userInfo.Username);
                if(getUser == null || !getUser.Status)
                {
                    return new UserManagerResponse()
                    {
                        Message = "Username isn't existed",                       
                        IsSuccess = false,
                        UserInfo = userInfo
                    };
                }              
                var refreshTokenObjInDB = getUser?.Data?.ReFreshToken;
                if (String.IsNullOrEmpty(refreshTokenObjInDB) 
                    || !refreshTokenObjInDB.Equals(userInfo.RefreshToken)
                    || getUser.Data.ExpiryDate <= DateTime.Now)
                {                    
                    return new UserManagerResponse()
                    {
                        Message = "token or refresh token is invalid",
                        IsSuccess = false,
                        UserInfo = userInfo
                    };
                };               
                
                userInfo.RefreshToken = GenerateRefreshToken();
                getUser.Data.ReFreshToken = userInfo.RefreshToken;
                getUser.Data.ExpiryDate = DateTime.Now.AddMonths(1);
                var updateData = new List<User>();
                updateData.Add(getUser.Data);
                _userService?.Update(updateData);
                
                userInfo.Token = GenerateToken(userInfo);
                return new UserManagerResponse()
                {
                    Message = "refresh token successfully",
                    IsSuccess = true,
                    UserInfo = userInfo
                };
            }
            return new UserManagerResponse()
            {
                Message = "refresh token failed",
                IsSuccess = false,
                UserInfo = userInfo
            };
        }
        //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        //{           
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    SecurityToken securityToken;
        //    var principal = tokenHandler.ValidateToken(token, tokenValiPara, out securityToken);
        //    var jwtSecurityToken = securityToken as JwtSecurityToken;
        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        throw new SecurityTokenException("Invalid token");
        //    return principal;
        //}
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


    }
}
