using Microsoft.AspNetCore.Mvc;

namespace clone_oblt.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // This renders your frontend view
        }
    }
}
