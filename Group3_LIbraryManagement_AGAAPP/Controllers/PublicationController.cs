using Microsoft.AspNetCore.Mvc;
using Group3_LIbraryManagement_AGAAPP.Models;
using System.Linq;
using Group3_LIbraryManagement_AGAAPP.Data;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class PublicationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publication
        public IActionResult Index(string searchQuery)
        {
            var publications = _context.Publications.AsQueryable(); // Start with all publications

            // Filter publications based on the search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                publications = publications.Where(p =>
                    p.Title.Contains(searchQuery) ||
                    p.Author.Contains(searchQuery) ||
                    p.ISBN_Code.Contains(searchQuery) ||
                    p.Publisher.Contains(searchQuery) ||
                    p.Genre.Contains(searchQuery) ||
                    p.Publication_Type.Contains(searchQuery) ||
                    p.Publication_Language.Contains(searchQuery) ||
                    p.Book_Edition.Contains(searchQuery) ||
                    p.Description.Contains(searchQuery));
            }

            // Pass the search query back to the view
            ViewBag.SearchQuery = searchQuery;

            return View(publications.ToList());
        }

        // GET: Publication/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publication publication)
        {
           
                _context.Publications.Add(publication);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Publication created successfully!";
                return RedirectToAction(nameof(Index));
          
        }

        // GET: Publication/Edit/5
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = _context.Publications.Find(id);
            if (publication == null)
            {
                return NotFound();
            }
            return View(publication);
        }

        // POST: Publication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Publication publication)
        {
            if (id != publication.Id)
            {
                return NotFound();
            }

           
                _context.Publications.Update(publication);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Publication updated successfully!";
                return RedirectToAction(nameof(Index));
           
        }

        // GET: Publication/Delete/5
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = _context.Publications.Find(id);
            if (publication == null)
            {
                return NotFound();
            }

            _context.Publications.Remove(publication);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Publication deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}