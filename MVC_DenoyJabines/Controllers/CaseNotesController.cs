using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;

namespace MVC_DenoyJabines.Controllers
{
    public class CaseNotesController : Controller
    {
        private readonly AppDbContext _context;

        public CaseNotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CaseNotes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.CaseNote.Include(c => c.Student);
            return View(await appDbContext.ToListAsync());
        }

        // GET: CaseNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNote = await _context.CaseNote
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseNote == null)
            {
                return NotFound();
            }

            return View(caseNote);
        }

        // GET: CaseNotes/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "StuID", "Address");
            return View();
        }

        // POST: CaseNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,Background,CounselingApproach,CounselingGoals,Comments,Recommendations,CreatedAt")] CaseNote caseNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "StuID", "Address", caseNote.StudentId);
            return View(caseNote);
        }

        // GET: CaseNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNote = await _context.CaseNote.FindAsync(id);
            if (caseNote == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "StuID", "Address", caseNote.StudentId);
            return View(caseNote);
        }

        // POST: CaseNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Background,CounselingApproach,CounselingGoals,Comments,Recommendations,CreatedAt")] CaseNote caseNote)
        {
            if (id != caseNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseNoteExists(caseNote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "StuID", "Address", caseNote.StudentId);
            return View(caseNote);
        }

        // GET: CaseNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNote = await _context.CaseNote
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseNote == null)
            {
                return NotFound();
            }

            return View(caseNote);
        }

        // POST: CaseNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseNote = await _context.CaseNote.FindAsync(id);
            if (caseNote != null)
            {
                _context.CaseNote.Remove(caseNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseNoteExists(int id)
        {
            return _context.CaseNote.Any(e => e.Id == id);
        }
    }
}
