using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;

namespace MVC_DenoyJabines.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        // Inject database context
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Display list of all users
        public async Task<IActionResult> UsersIndex()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Edit user page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Save edited user details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(
            "UserId,FirstName,MiddleName,LastName,Username,Email,ContactNumber,Role")] Users updatedUser)
        {
            if (id != updatedUser.UserId)
                return NotFound();

            // Exclude fields not editable here
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("IsActive");

            if (!ModelState.IsValid)
                return View(updatedUser);

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            // Update editable fields only; IsActive is untouched
            user.FirstName = updatedUser.FirstName;
            user.MiddleName = updatedUser.MiddleName;
            user.LastName = updatedUser.LastName;
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.ContactNumber = updatedUser.ContactNumber;
            user.Role = updatedUser.Role;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersIndex));
        }

        // GET: Delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Soft delete user (set IsActive to false)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user != null)
            {
                user.IsActive = false; // Soft delete
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersIndex));
        }

        // Helper: Check if user exists
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }

        // POST: Reactivate a soft-deleted user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reactivate(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            user.IsActive = true; // Reactivate user
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(UsersIndex));
        }
    }
}