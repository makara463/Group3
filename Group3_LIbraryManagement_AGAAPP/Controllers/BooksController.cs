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
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Books.Include(b => b.Publication);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Publication)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Title");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ISBN_Code,PublicationId,ShelfLocation,BookCondition,Status")] Book book)
        {       _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book created successfully!";
                return RedirectToAction(nameof(Index));
           
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Title", book.PublicationId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,ISBN_Code,PublicationId,ShelfLocation,BookCondition,Status")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Book updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Publication)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Book not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
        public IActionResult view_book_stock()
        {
            var books = _context.Books.Where(b => b.Status == "Available").ToList();
            return View("book_stock",books);
        }
    }
}