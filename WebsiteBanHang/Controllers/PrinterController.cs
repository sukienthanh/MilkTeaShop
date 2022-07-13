using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MilkTeaShop.Models;
using MilkTeaShop.Services;

namespace MilkTeaShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        private readonly IPrinter _service;
        public PrinterController(IPrinter _service)
        {
            this._service = _service;
        }
        [HttpPost]
        public IActionResult Print(List<PrinterModel> model)
        {
            var IP = "192.168.1.62";
            var port = 9100;
             _service.Print(model, IP, port);
            return Ok();
        }
    }
}

