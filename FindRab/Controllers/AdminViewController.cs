using FindRab.DataContext;
using FindRab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FindRab.Controllers
{
    [Authorize]
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
            ViewData["HideHeader"] = true;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserRed(int page = 1)
        {
            int pageSize = 15;
            var users = await _context.UserM.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.UserM.Count() / pageSize);
            ViewBag.CurrentPage = page;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int userId)
        {
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 1 || userId == currentUserId)
            {
                TempData["ErrorMessage"] = "Нельзя изменить роль этого пользователя.";
            }
            else
            {
                var user = await _context.UserM.FindAsync(userId);
                if (user != null)
                {
                    user.Role = (user.Role == 1) ? 2 : 1;
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("UserRed");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 1 || userId == currentUserId)
            {
                TempData["ErrorMessage"] = "Нельзя удалить этого пользователя.";
            }
            else
            {
                var user = await _context.UserM.FindAsync(userId);
                if (user != null)
                {
                    _context.UserM.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("UserRed");
        }

        [HttpPost]
        public async Task<IActionResult> BulkChangeRole(int[] selectedUsers)
        {
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (selectedUsers.Contains(1) || selectedUsers.Contains(currentUserId))
            {
                TempData["ErrorMessage"] = "Нельзя изменить роль одного из выбранных пользователей.";
                return RedirectToAction("UserRed");
            }

            var users = await _context.UserM.Where(u => selectedUsers.Contains(u.UserID)).ToListAsync();
            foreach (var user in users)
            {
                user.Role = (user.Role == 1) ? 2 : 1;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("UserRed");
        }

        [HttpPost]
        public async Task<IActionResult> BulkDelete(int[] selectedUsers)
        {
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (selectedUsers.Contains(1) || selectedUsers.Contains(currentUserId))
            {
                TempData["ErrorMessage"] = "Нельзя удалить одного из выбранных пользователей.";
                return RedirectToAction("UserRed");
            }

            var users = await _context.UserM.Where(u => selectedUsers.Contains(u.UserID)).ToListAsync();
            _context.UserM.RemoveRange(users);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserRed");
        }
    }
}
