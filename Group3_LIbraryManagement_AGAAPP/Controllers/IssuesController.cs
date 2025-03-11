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
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Issues.Include(i => i.Book).Include(i => i.Student);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var issue = await _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }
        public IActionResult Create()
        {
            var availableBooks = _context.Books.Where(b => b.Status == "Available").ToList();
            ViewData["BookId"] = new SelectList(availableBooks, "Id", "Title");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,StudentId,IssueDate,ReturnDate,Status")] Issue issue)
        {
                _context.Add(issue);
                var book = await _context.Books.FindAsync(issue.BookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = "The specified book does not exist.";
                    return View(issue); 
                }
                book.Status = "Issued";
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Issue created successfully!";
                return RedirectToAction(nameof(Index));
           
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", issue.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", issue.StudentId);
            return View(issue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,BookId,StudentId,IssueDate,ReturnDate,Status")] Issue issue)
        {
            if (id != issue.Id)
            {
                return NotFound();
            }
                try
                {
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Issue updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.Id))
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
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue != null)
            {
                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Issue deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Issue not found.";
            }

            return RedirectToAction(nameof(Index));
        }
        private bool IssueExists(string id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }
    }
}