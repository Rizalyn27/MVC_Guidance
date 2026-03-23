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

       
        private bool IsCounselorOrAdmin()
        {
            return User.IsInRole("Admin") || User.IsInRole("Counselor");
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string status)
        {
            var appointments = _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Counselor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
                appointments = appointments.Where(a => a.Status == status);

            ViewBag.CurrentStatus = status;
            ViewBag.IsCounselorOrAdmin = IsCounselorOrAdmin();

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Details/5 — Counselors & Admins only
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Counselor)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create — Everyone
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,CounselorID,AppointmentDate,AppointmentType,Notes")] Appointment appointment)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Counselor");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                appointment.Status = "Pending";
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(appointment.StudentID, appointment.CounselorID);
            return View(appointment);
        }

        // GET: Appointments/Edit/5 — Counselors & Admins only
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

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,StudentID,CounselorID,AppointmentDate,AppointmentType,Notes,Status")] Appointment appointment)
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
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentID))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(appointment.StudentID, appointment.CounselorID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5 — Counselors & Admins only
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Counselor)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5 — Soft delete (Cancelled)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsCounselorOrAdmin())
                return RedirectToAction(nameof(Index));

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                _context.Update(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns(int? selectedStudent = null, int? selectedCounselor = null)
        {
            ViewBag.StudentID = new SelectList(
                _context.Students
                    .Where(s => s.StuStatus)
                    .Select(s => new { s.StuID, FullName = s.StuFName + " " + s.StuLName }),
                "StuID", "FullName", selectedStudent);

            ViewBag.CounselorID = new SelectList(
                _context.User
                    .Where(u => u.Role == "Counselor" && u.IsActive),
                "UserId", "Username", selectedCounselor);

            ViewBag.AppointmentTypes = new SelectList(new[]
            {
                "Consultation", "Follow-up", "Crisis Intervention",
                "Career Counseling", "Academic Concern", "Personal/Social"
            });

            ViewBag.StatusOptions = new SelectList(new[]
            {
                "Pending", "Completed", "Cancelled"
            });
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}