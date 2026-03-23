using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;
using System.Diagnostics;

namespace MVC_DenoyJabines.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; // ? add this

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Counselor")]
        public IActionResult Home()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Counselor")]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentsHome()
        {
            var username = User.Identity?.Name;
            var result = await _context.Students
                .Where(s => s.Email == username || s.StuLRN == username)
                .ToListAsync();

            return View(result ?? new List<Students>());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}