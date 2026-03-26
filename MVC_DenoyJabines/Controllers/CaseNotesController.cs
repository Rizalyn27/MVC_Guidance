using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;

namespace MVC_DenoyJabines.Controllers
{
    // Only users with Counselor or Admin role can access this controller
    [Authorize(Roles = "Counselor,Admin")]
    public class CaseNotesController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject database context
        public CaseNotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CaseNotes
        // List all case notes, newest first
        public async Task<IActionResult> Index()
        {
            var caseNotes = await _context.CaseNotes
                .Include(c => c.Student) // Include related Student info
                .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt) // Sort by latest updated or created date
                .ToListAsync();

            return View(caseNotes);
        }

        // GET: CaseNotes/Details/5
        // View details of a specific case note
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var caseNote = await _context.CaseNotes
                .Include(c => c.Student)
                .FirstOrDefaultAsync(c => c.CaseNoteId == id);

            if (caseNote == null) return NotFound();

            return View(caseNote);
        }

        // GET: CaseNotes/Create
        // Show the create form
        public IActionResult Create()
        {
            // Pass active students to the view for dropdown selection
            ViewBag.Students = _context.Students
                .Where(s => s.StuStatus) // Only active students
                .OrderBy(s => s.StuLName)
                .ToList();

            return View();
        }

        // POST: CaseNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StuID,BackgroundOfCase,CounselingApproach,CounselingGoals,Comments,Recommendations")] CaseNotes caseNote)
        {
            // Remove navigation property from validation
            ModelState.Remove("Student");

            if (ModelState.IsValid)
            {
                caseNote.CreatedAt = DateTime.Now; // Set creation timestamp
                _context.Add(caseNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate students if validation fails
            ViewBag.Students = _context.Students
                .Where(s => s.StuStatus)
                .OrderBy(s => s.StuLName)
                .ToList();

            return View(caseNote);
        }

        // GET: CaseNotes/Edit/5
        // Show edit form for existing case note
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var caseNote = await _context.CaseNotes
                .Include(c => c.Student)
                .FirstOrDefaultAsync(c => c.CaseNoteId == id);

            if (caseNote == null) return NotFound();

            return View(caseNote);
        }

        // POST: CaseNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseNoteId,StuID,BackgroundOfCase,CounselingApproach,CounselingGoals,Comments,Recommendations")] CaseNotes caseNote)
        {
            if (id != caseNote.CaseNoteId) return NotFound();

            ModelState.Remove("Student"); // Remove navigation property from validation

            if (ModelState.IsValid)
            {
                // Fetch existing record
                var existing = await _context.CaseNotes.FindAsync(id);
                if (existing == null) return NotFound();

                // Update editable fields
                existing.BackgroundOfCase = caseNote.BackgroundOfCase;
                existing.CounselingApproach = caseNote.CounselingApproach;
                existing.CounselingGoals = caseNote.CounselingGoals;
                existing.Comments = caseNote.Comments;
                existing.Recommendations = caseNote.Recommendations;
                existing.UpdatedAt = DateTime.Now; // Set updated timestamp

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(caseNote);
        }

        // GET: CaseNotes/Delete/5
        // Show delete confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var caseNote = await _context.CaseNotes
                .Include(c => c.Student)
                .FirstOrDefaultAsync(c => c.CaseNoteId == id);

            if (caseNote == null) return NotFound();

            return View(caseNote);
        }

        // POST: CaseNotes/Delete/5
        // Confirm deletion of a case note
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseNote = await _context.CaseNotes.FindAsync(id);
            if (caseNote != null)
                _context.CaseNotes.Remove(caseNote);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a case note exists
        private bool CaseNoteExists(int id)
        {
            return _context.CaseNotes.Any(e => e.CaseNoteId == id);
        }
    }
}