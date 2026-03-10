using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MVC_DenoyJabines.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        // Make the method async and return Task<IActionResult>
        public async Task<IActionResult> Profile()
        {
            // Get all students from database
            var students = await _context.Students.ToListAsync();

            // Pass the list to the view
            return View(students);
        }
    }
}