using Microsoft.AspNetCore.Mvc;

namespace FindRab.Controllers
{
    public class AdminViewController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
