using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using WebClient.Models;
using WebClient.ApiClient;

namespace WebClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthClient _client;
        public AuthController(IAuthClient client)
        {
            _client = client;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup( [FromBody]UserRegister data)
        {
            if (ModelState.IsValid) {
                
                var result = await _client.Signup(data);
                if (result.IsSuccess && result.UserInfo != null)
                    HttpContext.Session.Set<UserInfo>(result.UserInfo);
                return Ok(result);
            }
            else
            {
                var errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage);
                return Ok(new UserManagerResponse("Model state invalid", false, errors));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            if (user is not null)
            {
                var result = await _client.Login(user);
                if (result.IsSuccess && result.UserInfo != null)
                    HttpContext.Session.Set<UserInfo>(result.UserInfo);
                return Ok(result);
            }
            var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
            return Ok(new UserManagerResponse("Model state invalid", false, errors));
        }

    }
}
