using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindRab.Models;

using FindRab.models;
using FindRab.DataContext;

namespace FindRab.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly BDContext _context;

        public ApplicationsController(BDContext context)
        {
            _context = context;
        }

        // GET: Applications/Create
        public IActionResult Create(int jobId)
        {
            ViewData["JobId"] = jobId;
            return View();
        }

        // POST: Applications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,CoverLetter")] JobApplication application)
        {
            if (ModelState.IsValid)
            {
                application.UserId = User.Identity.Name;
                application.AppliedAt = DateTime.Now;
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Jobs", new { id = application.JobId });
            }
            return View(application);
        }
    }
}


