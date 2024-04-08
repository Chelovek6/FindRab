using Microsoft.AspNetCore.Mvc;

namespace FindRab.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
