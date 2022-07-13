using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using MilkTeaShop.Services;

namespace MilkTeaShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalog _service;
        public CatalogController(ICatalog _service)
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
        public async Task<IActionResult> GetById([FromQuery] int? id = null)
        {
            var result = await _service.Get(id);
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]int[] id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] IEnumerable<Catalog> data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] IEnumerable<Catalog> data)
        {
            var result = await _service.Add(data);
            return Ok(result);
        }
    }
}
