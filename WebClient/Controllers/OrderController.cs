using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using WebClient.ApiClient;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrder _orderClient;
        private readonly IAuthClient _authClient;
        public OrderController(IOrder _client, IAuthClient _authClient)
        {
            this._orderClient = _client;
            this._authClient = _authClient;
        }
        // GET: CategoriesController
        public ActionResult Index()

        {
            var userInfo = HttpContext.Session.Get<UserInfo>("s3cr3cK3y");
            if (userInfo != null && userInfo.Role != "Customer" && userInfo.Role != "Guest")
            {
                return View();
            }
            return Unauthorized();
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            if (ModelState.IsValid)
            {
                var result = await _orderClient.GetList();
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderClient.GetById(id);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int[] id)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<UserInfo>("s3cr3cK3y");
                if (userInfo != null)
                {
                    var result = await _orderClient.Delete(id, userInfo.Token);
                    return Ok(result);
                }
                return Unauthorized();
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] List<Order> data)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<UserInfo>("s3cr3cK3y");
                if (userInfo != null)
                {
                    var result = await _orderClient.Update(data, userInfo.Token);
                    return Ok(result);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] List<Order> data)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<UserInfo>("s3cr3cK3y");
                if (userInfo != null)
                {
                    var result = await _orderClient.Add(data, userInfo.Token);
                    return Ok(result);
                }
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            return Ok(await _orderClient.SendMailAsync());
        }
    }
}
