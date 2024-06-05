﻿using FindRab.DataContext;
using FindRab.models;
using FindRab.Models;
using FindRab.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FindRab.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly BDContext _context;

        public JobsController(BDContext context)
        {
            _context = context;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var vacancies = await _context.VacanciesM.ToListAsync();
            var model = vacancies.Select(v => new VacancyViewModel
            {
                VacancyId = v.VacancyId,
                Title = v.Title,
                Description = v.Description,
                Education = v.Education,
                Salary = v.Salary,
                UserId = v.UserId,
                Phone = v.Phone,
                Email = v.Email
            }).ToList();

            return View(model);
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.VacanciesM.FirstOrDefaultAsync(v => v.VacancyId == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            var model = new VacancyViewModel
            {
                VacancyId = vacancy.VacancyId,
                Title = vacancy.Title,
                Description = vacancy.Description,
                Education = vacancy.Education,
                Salary = vacancy.Salary,
                UserId = vacancy.UserId,
                Phone = vacancy.Phone,
                Email = vacancy.Email
            };

            return View(model);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacancyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = User.Identity.Name;
                var user = await _context.UserM.FirstOrDefaultAsync(u => u.Username == currentUser);

                if (user == null)
                {
                    // Если текущий пользователь не найден, перенаправляем на страницу логина или выводим ошибку
                    ModelState.AddModelError("", "Пользователь не найден. Пожалуйста, войдите в систему.");
                    return RedirectToAction("Login", "Account");
                }

                var vacancy = new Vacancy
                {
                    Title = model.Title,
                    Description = model.Description,
                    Education = model.Education,
                    Salary = model.Salary,
                    UserId = user.UserID, // Устанавливаем UserId текущего пользователя
                    Phone = model.Phone,
                    Email = model.Email
                };

                try
                {
                    _context.Add(vacancy);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Не удалось сохранить изменения. Попробуйте еще раз, и если проблема не исчезнет, обратитесь к системному администратору.");
                    // Логирование исключения
                    Console.WriteLine(ex.InnerException?.Message);
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: Jobs/UserVacancies
        public async Task<IActionResult> UserVacancies()
        {
            var currentUser = User.Identity.Name;
            var user = await _context.UserM.FirstOrDefaultAsync(u => u.Username == currentUser);
            if (user == null)
            {
                return NotFound();
            }

            var userVacancies = await _context.VacanciesM
                .Where(v => v.UserId == user.UserID)
                .ToListAsync();

            var model = userVacancies.Select(v => new VacancyViewModel
            {
                VacancyId = v.VacancyId,
                Title = v.Title,
                Description = v.Description,
                Education = v.Education,
                Salary = v.Salary,
                UserId = v.UserId,
                Phone = v.Phone,
                Email = v.Email
            }).ToList();

            return View(model);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.VacanciesM.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            var model = new VacancyViewModel
            {
                VacancyId = vacancy.VacancyId,
                Title = vacancy.Title,
                Description = vacancy.Description,
                Education = vacancy.Education,
                Salary = vacancy.Salary,
                UserId = vacancy.UserId,
                Phone = vacancy.Phone,
                Email = vacancy.Email
            };

            return View(model);
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VacancyViewModel model)
        {
            if (id != model.VacancyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vacancy = await _context.VacanciesM.FindAsync(id);
                    if (vacancy == null)
                    {
                        return NotFound();
                    }

                    vacancy.Title = model.Title;
                    vacancy.Description = model.Description;
                    vacancy.Education = model.Education;
                    vacancy.Salary = model.Salary;
                    vacancy.UserId = model.UserId;
                    vacancy.Phone = model.Phone;
                    vacancy.Email = model.Email;

                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacancyExists(model.VacancyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserVacancies));
            }
            return View(model);
        }

        private bool VacancyExists(int id)
        {
            return _context.VacanciesM.Any(e => e.VacancyId == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Apply(int vacancyId)
        {
            try
            {
                var currentUser = User.Identity.Name;
                Console.WriteLine($"Current user: {currentUser}");

                var user = await _context.UserM.FirstOrDefaultAsync(u => u.Username == currentUser);
                if (user == null)
                {
                    Console.WriteLine("User not found");
                    return NotFound();
                }

                var vacancy = await _context.VacanciesM.FirstOrDefaultAsync(v => v.VacancyId == vacancyId);
                if (vacancy == null)
                {
                    Console.WriteLine("Vacancy not found");
                    return NotFound();
                }

                var existingApplication = await _context.JobApplicationsM
                    .FirstOrDefaultAsync(a => a.UserId == user.UserID && a.VacancyId == vacancyId);

                if (existingApplication == null)
                {
                    var application = new JobApplication
                    {
                        UserId = user.UserID,
                        VacancyId = vacancyId
                    };

                    _context.JobApplicationsM.Add(application);
                    vacancy.ApplicationCount += 1;
                    await _context.SaveChangesAsync();
                }

                var viewModel = new VacancyViewModel
                {
                    VacancyId = vacancy.VacancyId,
                    Title = vacancy.Title,
                    Description = vacancy.Description,
                    Education = vacancy.Education,
                    Salary = vacancy.Salary,
                    UserId = vacancy.UserId,
                    Phone = vacancy.Phone,
                    Email = vacancy.Email,
                    ApplicationCount = vacancy.ApplicationCount
                };

                ViewBag.Phone = vacancy.Phone;
                ViewBag.Email = vacancy.Email;

                return View("Details", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }




    }
}
