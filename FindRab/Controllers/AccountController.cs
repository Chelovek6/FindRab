using FindRab.Models;
using FindRab.models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography; // добавляем для использования SHA256
using System.Threading.Tasks; // добавляем для использования Task
using System.Collections.Generic; // добавляем для использования List


namespace FindRab.Controllers
{
    public class AccountController : Controller
    {
        private BDContext db;
        public AccountController(BDContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Autorization()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Autorization(User model)
        {
            SHA256 Hash = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(model.Username + model.Password);
            byte[] hash = Hash.ComputeHash(inputBytes);
            model.Password = Convert.ToHexString(hash);

            if (ModelState.IsValid)
            {
                User sec = await db.UserM.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
                if (sec != null)
                {
                    await Authenticate(model.Username); // аутентификация
                    Console.WriteLine("GoodYeeees");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(User model)
        {
            SHA256 Hash = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(model.Username + model.Password);
            byte[] hash = Hash.ComputeHash(inputBytes);
            model.Password = Convert.ToHexString(hash);

            if (ModelState.IsValid)
            {
                User sec = await db.UserM.FirstOrDefaultAsync(u => u.UserID == model.UserID && u.Username == model.Username && u.Password == model.Password);
                if (sec == null)
                {
                    // добавляем пользователя в бд
                    db.UserM.Add(new User { UserID = model.UserID, Username = model.Username, Password = model.Password });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Username); // аутентификация

                    return RedirectToAction("PostRegistration");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model); 
        }
        public async Task<IActionResult> PostRegistration(User model)
        {
            if (ModelState.IsValid)
            {
                // Поиск пользователя в базе данных по имени пользователя и паролю
                User existingUser = await db.UserM.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (existingUser == null)
                {
                    // Если пользователя с такими данными не существует, добавляем его в базу данных
                    db.UserM.Add(model);
                    await db.SaveChangesAsync();

                    // После успешной регистрации проводим аутентификацию пользователя
                    await Authenticate(model.Username);
                    await Authenticate(model.Username);

                    return RedirectToAction("Home", "index");
                }
                else
                {
                    // Если пользователь с такими данными уже существует, выдаем сообщение об ошибке
                    ModelState.AddModelError("", "Пользователь с такими данными уже существует");
                }
            }
            // Если модель не прошла валидацию, возвращаем представление с моделью, чтобы пользователь мог исправить ошибки
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Autorization", "Account");
        }
    }
}
