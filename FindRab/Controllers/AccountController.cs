using Microsoft.AspNetCore.Mvc;
using FindRab.DataContext;
using FindRab.Models;
using FindRab.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using FindRab.models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FindRab.Controllers
{
    public class AccountController : Controller
    {
        private readonly BDContext _context;

        public AccountController(BDContext context)
        {
            _context = context;
        }

        // POST: /Account/Login
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _context.UserM
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    var roleCode = user.Role;

                    if (roleCode == 1)
                    {
                        return RedirectToAction("Index", "AdminView");
                    }
                    else if (roleCode == 2)
                    {
                        return RedirectToAction("Index", "Menu");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                }
            }

            return View(model);
        }

        // POST: /Account/Register
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверка на уникальность логина
                if (await _context.UserM.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Пользователь с таким логином уже существует");
                    return View(model);
                }

                // Создание нового пользователя с ролью "User"
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    Role = 2
                };

                // Сохранение пользователя в базе данных
                _context.Add(user);
                await _context.SaveChangesAsync();

                // Перенаправление на главную страницу для пользователя
                return RedirectToAction("Index", "Menu");
            }
            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
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
        // GET: /Account/RegistrationSuccess
        public IActionResult RegistrationSuccess()
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}