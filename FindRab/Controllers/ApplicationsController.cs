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

        
        
    }
}


