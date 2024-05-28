﻿using FindRab.DataContext;
using FindRab.models;
using FindRab.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FindRab.Controllers
{
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
                UserId = v.UserId
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
                UserId = vacancy.UserId
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
                var vacancy = new Vacancy
                {
                    Title = model.Title,
                    Description = model.Description,
                    Education = model.Education,
                    Salary = model.Salary,
                    UserId = model.UserId
                };

                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                UserId = v.UserId
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
                UserId = vacancy.UserId
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
    }
}
