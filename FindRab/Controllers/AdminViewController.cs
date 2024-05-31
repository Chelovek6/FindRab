using FindRab.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindRab.Models;

namespace FindRab.Controllers
{
    
    public class AdminViewController : Controller
    {
        private readonly BDContext _context;

        public AdminViewController(BDContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserRed()
        {
            var users = await _context.UserM.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = await _context.UserM.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int userId)
        {
            if (userId == 16) // Проверка на id пользователя 16
            {
                TempData["ErrorMessage"] = "Нельзя изменить роль первого пользователя.";
            }
            else
            {
                var user = await _context.UserM.FindAsync(userId);
                if (user != null)
                {
                    // Смена роли пользователя
                    user.Role = (user.Role == 1) ? 2 : 1; // Если роль пользователя 1, меняем на 2 и наоборот
                    await _context.SaveChangesAsync(); // Сохранение изменений в базе данных
                }
            }
            return RedirectToAction("UserRed");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            if (userId == 16) // Проверка на id пользователя 16
            {
                TempData["ErrorMessage"] = "Нельзя удалить первого пользователя.";
                return RedirectToAction("Index");
            }

            var user = await _context.UserM.FindAsync(userId);
            if (user != null)
            {
                _context.UserM.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UserRed");
        }

    }
}