using FindRab.models;
using FindRab.Models;
using FindRab.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FindRab.Controllers
{
    public class HomeController : Controller
    {
        BDContext db;
        public HomeController(BDContext context)
        {
            db = context;
        }
        public IActionResult Autorization()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Reg(string userName, string userPas)
        {

            return new JsonResult(userName, userPas);
        }
        public IActionResult Reg()
        {
            return View();
        }
        public IActionResult Autorization(SecurityModel auto)
        {
            auto.Pass_word = auto.Code(auto.Log_in + auto.Pass_word);
            if (db.Find(auto.Log_in, auto.Pass_word))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Autorization");
            }
        }
    }
}

