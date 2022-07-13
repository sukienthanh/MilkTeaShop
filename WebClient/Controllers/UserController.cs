using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
