using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using MilkTeaShop.Services;

namespace MilkTeaShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _service;
        public UserController(IUser _service)
        {
            this._service = _service;
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _service.GetList();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int? id = null,string? username = "", string? email = "")
        {
            var result = await _service.Get(id, username, email);
            return Ok(result);
        }
        [Authorize("Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delele([FromBody] int[] ids)
        {
            var result = await _service.Delete(ids);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] IEnumerable<User> data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] IEnumerable<User> data)
        {
            var result = await _service.Add(data);
            return Ok(result);
        }
    }
}
