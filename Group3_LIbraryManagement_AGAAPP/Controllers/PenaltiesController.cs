using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group3_LIbraryManagement_AGAAPP.Data;
using Group3_LIbraryManagement_AGAAPP.Models;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class PenaltiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PenaltiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Penalties
        public async Task<IActionResult> Index()
        {
            var penalties = await _context.Penalties
                .Include(p => p.Issue)
                .Include(p => p.Student)
                .ToListAsync();
            return View(penalties);
        }

        // GET: Penalties/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var penalty = await _context.Penalties
                .Include(p => p.Issue)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // GET: Penalties/Create
        public IActionResult Create()
        {
            ViewData["IssueId"] = new SelectList(_context.Issues, "Id", "BookId");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: Penalties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,IssueId,PenaltyType,Amount,PenaltyDate,PaymentStatus,PaymentDate")] Penalty penalty)
        {
           
                _context.Add(penalty);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Penalty created successfully!";
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Penalties/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalties.FindAsync(id);
            if (penalty == null)
            {
                return NotFound();
            }

            ViewData["IssueId"] = new SelectList(_context.Issues, "Id", "BookId", penalty.IssueId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", penalty.StudentId);
            return View(penalty);
        }

        // POST: Penalties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,StudentId,IssueId,PenaltyType,Amount,PenaltyDate,PaymentStatus,PaymentDate")] Penalty penalty)
        {
            if (id != penalty.Id)
            {
                return NotFound();
            }

          
                try
                {
                    _context.Update(penalty);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Penalty updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PenaltyExists(penalty.Id))
                    {
                        TempData["ErrorMessage"] = "Penalty not found.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Penalties/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalties
                .Include(p => p.Issue)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // POST: Penalties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var penalty = await _context.Penalties.FindAsync(id);
            if (penalty != null)
            {
                _context.Penalties.Remove(penalty);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Penalty deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Penalty not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PenaltyExists(string id)
        {
            return _context.Penalties.Any(e => e.Id == id);
        }
    }
}