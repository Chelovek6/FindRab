using Microsoft.AspNetCore.Mvc;
using FindRab.DataContext;
using FindRab.Models;
using FindRab.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FindRab.models;

namespace FindRab.Controllers
{
    public class AccountController : Controller
    {
        private readonly BDContext _context;

        public AccountController(BDContext context)
        {
            _context = context;
            _context.UserM.Load();
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["HideHeader"] = true;
            ViewData["HideFooter"] = true;
            return View();
        }

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
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                   
                    return RedirectToAction("Index", user.Role == 1 ? "AdminView" : "Menu");
                }

                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
            }
            ViewData["HideHeader"] = true;
            ViewData["HideFooter"] = true;
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["HideHeader"] = true;
            ViewData["HideFooter"] = true;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _context.UserM.AnyAsync(u => u.Username == model.Username);
                if (userExists)
                {
                    ModelState.AddModelError("Username", "Имя пользователя уже занято");
                    ViewData["HideHeader"] = true;
                    ViewData["HideFooter"] = true;
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    Role = 2
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                await Authenticate(user.Username);
               
                return RedirectToAction("Index", "Menu");
            }
            ViewData["HideHeader"] = true;
            ViewData["HideFooter"] = true;
            return View(model);
        }
        // /////////////////////////////////////////////////////////
        // GET: /Account/EditProfile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userName = User.Identity.Name;
            var user = await _context.UserM.FirstOrDefaultAsync(u => u.Username == userName);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                Username = user.Username,
                Password = user.Password
            };

            return View(model);
        }

        // POST: /Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var user = await _context.UserM.FirstOrDefaultAsync(u => u.Username == userName);

                if (user == null)
                {
                    return NotFound();
                }

                // Проверка уникальности имени пользователя, если оно изменилось
                if (model.Username != user.Username)
                {
                    var userExists = await _context.UserM.AnyAsync(u => u.Username == model.Username);
                    if (userExists)
                    {
                        ModelState.AddModelError("Username", "Имя пользователя уже занято");
                        return View(model);
                    }
                }

                user.Username = model.Username;
                user.Password = model.Password;
                _context.Update(user);
                await _context.SaveChangesAsync();

                // Обновление аутентификационного билета
                await SignInUser(user);

                return RedirectToAction("Index", user.Role == 1 ? "AdminView" : "Menu");
            }

            return View(model);
        }

        private async Task SignInUser(User user)
        {
            // Создание нового списка утверждений (claims)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            // Создание объекта ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(claims, "Login");

            // Создание объекта ClaimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Вход пользователя с помощью аутентификационного менеджера
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        // //////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        // GET: /Account/RegistrationSuccess
        public IActionResult RegistrationSuccess()
        {
            
            return View("~/Views/Menu/Index.cshtml");
        }
    }
}
