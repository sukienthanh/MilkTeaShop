using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using MilkTeaShop.Services;

namespace MilkTeaShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAbout _service;
        public AboutController(IAbout _service)
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
        public async Task<IActionResult> Get([FromQuery] int? id = null)
        {
            var result = await _service.Get(id);
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delele([FromBody] int[] ids)
        {
            var result = await _service.Delete(ids);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] IEnumerable<About> data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] IEnumerable<About> data)
        {
            var result = await _service.Add(data);
            return Ok(result);
        }
    }
}
