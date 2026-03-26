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
        private readonly AppDbContext _context;

        // Constructor to inject logger and database context
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Admin dashboard view
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            // Students statistics
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            ViewBag.ActiveStudents = await _context.Students.CountAsync(s => s.StuStatus);
            ViewBag.InactiveStudents = await _context.Students.CountAsync(s => !s.StuStatus);

            // User statistics
            ViewBag.TotalUsers = await _context.User.CountAsync();
            ViewBag.ActiveUsers = await _context.User.CountAsync(u => u.IsActive);

            // Appointment statistics
            ViewBag.TotalAppts = await _context.Appointments.CountAsync();
            ViewBag.PendingAppts = await _context.Appointments.CountAsync(a => a.Status == "Pending");
            ViewBag.ConfirmedAppts = await _context.Appointments.CountAsync(a => a.Status == "Confirmed");
            ViewBag.CompletedAppts = await _context.Appointments.CountAsync(a => a.Status == "Completed");
            ViewBag.CancelledAppts = await _context.Appointments.CountAsync(a => a.Status == "Cancelled");
            ViewBag.MissedAppts = await _context.Appointments.CountAsync(a => a.Status == "Missed");

            return View();
        }

        // Counselor dashboard view
        [Authorize(Roles = "Counselor")]
        public async Task<IActionResult> Home()
        {
            // Students statistics
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            ViewBag.ActiveStudents = await _context.Students.CountAsync(s => s.StuStatus);
            ViewBag.InactiveStudents = await _context.Students.CountAsync(s => !s.StuStatus);

            // Appointment statistics
            ViewBag.TotalAppts = await _context.Appointments.CountAsync();
            ViewBag.PendingAppts = await _context.Appointments.CountAsync(a => a.Status == "Pending");
            ViewBag.ConfirmedAppts = await _context.Appointments.CountAsync(a => a.Status == "Confirmed");
            ViewBag.CompletedAppts = await _context.Appointments.CountAsync(a => a.Status == "Completed");
            ViewBag.CancelledAppts = await _context.Appointments.CountAsync(a => a.Status == "Cancelled");
            ViewBag.MissedAppts = await _context.Appointments.CountAsync(a => a.Status == "Missed");

            return View();
        }

        // Student dashboard view
        [Authorize(Roles = "Student")]
        public IActionResult StudentsHome()
        {
            return View();
        }

        // Profile view accessible to Admin and Counselor
        [Authorize(Roles = "Admin,Counselor")]
        public IActionResult Profile()
        {
            return View();
        }

        // Redirect to Appointments index view
        [Authorize(Roles = "Admin,Counselor")]
        public IActionResult AppointmentIndex()
        {
            // Render the Appointments Index view directly
            return View("~/Views/Appointments/Index.cshtml");
        }

        // Error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Pass request ID for debugging
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}