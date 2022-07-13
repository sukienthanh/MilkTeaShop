using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using WebClient.ApiClient;


namespace WebClient.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalog _client;
        public CatalogController(ICatalog _client)
        {
            this._client = _client;
        }
        // GET: CategoriesController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            if (ModelState.IsValid)
            {
                var result = await _client.GetList();
                return Ok(result);
            } 
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _client.GetById(id);
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]int[] id)
        {
            if (ModelState.IsValid)
            {
                var result = await _client.Delete(id);
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]List<Catalog> data)
        {
            if (ModelState.IsValid)
            {
                var result = await _client.Update(data);
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost] 
        public async Task<IActionResult> Add([FromBody]List<Catalog> data)
        {
            if (ModelState.IsValid)
            {
                var result = await _client.Add(data);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
