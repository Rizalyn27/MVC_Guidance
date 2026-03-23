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

        // GET: Users list with filter
        public async Task<IActionResult> UsersIndex(string status)
        {
            var users = _context.User.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToLower() == "active")
                    users = users.Where(u => u.IsActive);
                else if (status.ToLower() == "inactive")
                    users = users.Where(u => !u.IsActive);
                // "all" = no filter
            }

            return View(await users.ToListAsync());
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

            ModelState.Remove("Role");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
                return View(updatedUser);

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersIndex));
        }

        // GET: Users/Delete/5 — confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5 — soft delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user != null)
            {
                user.IsActive = false;
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersIndex));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}