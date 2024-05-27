namespace FindRab.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    
    using global::FindRab.models;
    using global::FindRab.DataContext;

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
                return View(await _context.VacanciesM.ToListAsync());
            }

            // GET: Jobs/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Jobs/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Title,Description,Education,Salary")] Vacancy vacancy)
            {
                if (ModelState.IsValid)
                {
                    vacancy.CreatedBy = User.Identity.Name;
                    vacancy.CreatedAt = DateTime.Now;
                    _context.Add(vacancy);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(vacancy);
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
                return View(vacancy);
            }

            // POST: Jobs/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("VacancyId,Title,Description,Education,Salary")] Vacancy vacancy)
            {
                if (id != vacancy.VacancyId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(vacancy);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!VacancyExists(vacancy.VacancyId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(vacancy);
            }

            private bool VacancyExists(int id)
            {
                return _context.VacanciesM.Any(e => e.VacancyId == id);
            }
        }
    }

}
