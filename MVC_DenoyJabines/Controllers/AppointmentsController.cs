using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;

namespace MVC_DenoyJabines.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StudentAppointments
        public async Task<IActionResult> StudentAppointments()
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var appointments = await _context.Appointments
                .Where(a => a.UserId == currentUserId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,Email,ContactNumber,AppointmentDate,AppointmentType,Notes,Status")] Appointment appointment)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Counselor");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                // Get current logged-in user's ID from claims
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized();
                }

                appointment.UserId = int.Parse(userIdClaim);
                appointment.CreatedAt = DateTime.Now;
                appointment.Status = "Pending";

                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(appointment);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var appointment = await _context.Appointments.Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            PopulateDropdowns(appointment.StudentID, appointment.CounselorID);
            return View(appointment);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,FirstName,LastName,MiddleName,Email,ContactNumber,AppointmentDate,AppointmentType,Notes,Status,UserId,CreatedAt")] Appointment appointment)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            if (id != appointment.AppointmentID) return NotFound();

            ModelState.Remove("Student");
            ModelState.Remove("Counselor");

            if (ModelState.IsValid)
            {
                try
                {
                    appointment.UpdatedAt = DateTime.Now;
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Appointments.Any(a => a.AppointmentID == appointment.AppointmentID))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(StudentAppointments));
            }

            PopulateDropdowns(appointment.StudentID, appointment.CounselorID);
            return View(appointment);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();
            var appointment = await _context.Appointments.Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(StudentAppointments));
        }

        [Authorize(Roles = "Counselor,Admin")]
        public IActionResult Index()
            {
            return View();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}