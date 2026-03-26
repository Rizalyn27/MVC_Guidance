using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_DenoyJabines.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        // Only Counselor and Admin can see all appointments
        [Authorize(Roles = "Counselor,Admin")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Appointments.Include(a => a.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Appointments/StudentsAppt
        // Only Students can see their own appointments
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentsAppt()
        {
            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim == null) return Unauthorized(); // Check if user is logged in

            int userId = int.Parse(userIdClaim);

            // Fetch appointments only for the logged-in student
            var myAppointments = await _context.Appointments
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View(myAppointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Fetch appointment details including the User info
            var appointment = await _context.Appointments
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize]
        public IActionResult Create()
        {
            Appointment appointment = new Appointment();

            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim);
                var currentUser = _context.User.FirstOrDefault(u => u.UserId == userId);
                if (currentUser != null)
                {
                    appointment.FirstName = currentUser.FirstName;
                    appointment.LastName = currentUser.LastName;
                    appointment.MiddleName = currentUser.MiddleName;
                    appointment.Email = currentUser.Email;
                    appointment.ContactNumber = currentUser.ContactNumber;
                    appointment.UserId = currentUser.UserId;
                }
            }

            if (User.IsInRole("Admin") || User.IsInRole("Counselor"))
            {
                ViewBag.StudentUsers = _context.Students
                    .Where(u => u.StuStatus == true)
                    .Select(u => new {
                        UserId = u.StuID,
                        FirstName = u.StuFName,
                        LastName = u.StuLName,
                        MiddleName = u.StuMName,
                        Email = u.Email,
                        ContactNumber = u.Contact
                    })
                    .ToList();
            }

            appointment.Status = "Pending";
            appointment.AppointmentDate = DateTime.Now.AddHours(1);

            return View(appointment);
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,Email,ContactNumber,AppointmentDate,AppointmentType,Notes")] Appointment appointment)
        {
            var userIdClaim = User.FindFirstValue("UserId");
            if (userIdClaim == null) return Unauthorized();

            appointment.UserId = int.Parse(userIdClaim);
            appointment.CreatedAt = DateTime.Now;
            appointment.UpdatedAt = null;
            appointment.Status = "Pending";

            ModelState.Remove("UserId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");
            ModelState.Remove("User");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                return User.IsInRole("Student") ? RedirectToAction(nameof(StudentsAppt)) : RedirectToAction(nameof(Index));
            }

            // Re-populate on validation failure
            if (User.IsInRole("Admin") || User.IsInRole("Counselor"))
            {
                ViewBag.StudentUsers = _context.Students
                    .Where(u => u.StuStatus == true)
                    .Select(u => new {
                        UserId = u.StuID,
                        FirstName = u.StuFName,
                        LastName = u.StuLName,
                        MiddleName = u.StuMName,
                        Email = u.Email,
                        ContactNumber = u.Contact
                    })
                    .ToList();
            }

            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,FirstName,LastName,MiddleName,Email,ContactNumber,AppointmentDate,AppointmentType,Notes,Status")] Appointment appointment)
        {
            if (id != appointment.AppointmentID) return NotFound();

            // Remove properties not handled by form
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                // Fetch existing record to preserve UserId and CreatedAt
                var existing = await _context.Appointments.FindAsync(id);
                if (existing == null) return NotFound();

                // Update editable fields
                existing.FirstName = appointment.FirstName;
                existing.LastName = appointment.LastName;
                existing.MiddleName = appointment.MiddleName;
                existing.Email = appointment.Email;
                existing.ContactNumber = appointment.ContactNumber;
                existing.AppointmentDate = appointment.AppointmentDate;
                existing.AppointmentType = appointment.AppointmentType;
                existing.Notes = appointment.Notes;
                existing.Status = appointment.Status;
                existing.UpdatedAt = DateTime.Now; // Auto-set UpdatedAt

                await _context.SaveChangesAsync();

                return User.IsInRole("Student") ? RedirectToAction(nameof(StudentsAppt)) : RedirectToAction(nameof(Index));
            }

            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Fetch appointment including User info
            var appointment = await _context.Appointments
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if appointment exists
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }

        // POST: Appointments/UpdateStatus
        // Only Counselor and Admin can update appointment status
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = status;
            appointment.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}