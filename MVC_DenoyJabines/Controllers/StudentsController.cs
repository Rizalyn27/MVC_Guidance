using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_DenoyJabines.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        // Inject database context
        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students list
        public async Task<IActionResult> Index(string status)
        {
            var students = _context.Students.AsQueryable();

            // If logged-in user is a student, show only their own record
            if (User.IsInRole("Student"))
            {
                var username = User.Identity?.Name;

                var result = await students
                    .Where(s => s.Email == username
                             || s.StuLRN == username
                             || (s.StuFName + s.StuLName) == username)
                    .ToListAsync();

                return View("StudentIndex", result ?? new List<Students>());
            }

            // Filter for Counselor/Admin based on status
            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToLower() == "active")
                    students = students.Where(s => s.StuStatus);
                else if (status.ToLower() == "inactive")
                    students = students.Where(s => !s.StuStatus);
            }

            return View(await students.ToListAsync());
        }

        // GET: Student details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var students = await _context.Students.FirstOrDefaultAsync(m => m.StuID == id);
            if (students == null) return NotFound();

            return View(students);
        }

        // GET: Create student page
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
            "StuID,StuLRN,StuFName,StuLName,StuMName,StuStatus,Gender,Birthdate,Address,Contact,Email,Department,YearLevel,Section,Adviser,GuardianName,Relationship,GuardianContact,GuardianAddress,Reason,CaseType,CounselingStatus,Remarks")]
            Students students)
        {
            if (ModelState.IsValid)
            {
                // New student is active by default
                students.StuStatus = true;

                _context.Add(students);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(students);
        }

        // GET: Edit student page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var students = await _context.Students.FindAsync(id);
            if (students == null) return NotFound();

            return View(students);
        }

        // POST: Edit student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(
            "StuID,StuLRN,StuFName,StuLName,StuMName,StuStatus,Gender,Birthdate,Address,Contact,Email,Department,YearLevel,Section,Adviser,GuardianName,Relationship,GuardianContact,GuardianAddress,Reason,CaseType,CounselingStatus,Remarks")]
            Students students)
        {
            if (id != students.StuID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.StuID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(students);
        }

        // GET: Delete student confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var students = await _context.Students.FirstOrDefaultAsync(m => m.StuID == id);
            if (students == null) return NotFound();

            return View(students);
        }

        // POST: Soft delete student (mark inactive)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _context.Students.FindAsync(id);

            if (students != null)
            {
                // Soft delete
                students.StuStatus = false;
                _context.Update(students);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper: Check if student exists
        private bool StudentsExists(int id)
        {
            return _context.Students.Any(e => e.StuID == id);
        }

        // STUDENT PROFILE VIEW
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Profile()
        {
            var username = User.Identity?.Name;

            var result = await _context.Students
                .Where(s => s.Email == username || s.StuLRN == username)
                .ToListAsync();

            return View(result ?? new List<Students>());
        }
    }
}