using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;
using MilkTeaShop.Services;

namespace MilkTeaShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authen;      
        
        public AuthController(IAuth _authen)
        {
            this._authen = _authen;                  
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin data)
        {
            if (ModelState.IsValid)
            {
                var response = await _authen.Login(data);
                return Ok(response);
            }
            return BadRequest();
            
        }

        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] UserRegister data)
        {
            if (ModelState.IsValid)
            {
                var response = await _authen.Register(data);
                return Ok(response);
            }
            return BadRequest();
        }
        [Authorize]
        [HttpGet]
        public IActionResult CheckToken()
        {
            return Ok(true);
        }

        [HttpPost]
        public IActionResult CreateToken([FromBody] UserInfo user)
        {
            return Ok(_authen.GenerateToken (user));
        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] UserInfo user)
        {
            return  Ok(await _authen.RefreshToken(user));
        }

    }
}
