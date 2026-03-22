using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_DenoyJabines.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users list
        public async Task<IActionResult> UsersIndex()
        {
            var users = await _context.User.ToListAsync();
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email")] Users updatedUser)
        {
            if (id != updatedUser.UserId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(updatedUser);

            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            // Update only Username and Email
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;

            // Save changes
            await _context.SaveChangesAsync();

            // Redirect back to Users table
            return RedirectToAction(nameof(UsersIndex));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}