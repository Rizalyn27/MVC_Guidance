using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;

namespace MVC_DenoyJabines.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.User.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(Users user)
        {
            var existing = await _context.User.FindAsync(user.UserId);

            if (existing == null)
                return NotFound();

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.Role = user.Role;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
