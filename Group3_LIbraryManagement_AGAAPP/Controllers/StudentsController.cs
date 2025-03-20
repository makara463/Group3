using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group3_LIbraryManagement_AGAAPP.Data;
using Group3_LIbraryManagement_AGAAPP.Models;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchQuery)
        {
            // Log the search query for debugging
            Console.WriteLine($"Search Query: {searchQuery}");

            var studentsQuery = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Trim and convert the search query to lowercase for case-insensitive search
                searchQuery = searchQuery.Trim().ToLower();

                // Filter the students by Name or Email (case-insensitive)
                studentsQuery = studentsQuery.Where(s =>
                    s.Name.ToLower().Contains(searchQuery) ||
                    s.Gender.ToLower().Contains(searchQuery) ||
                    s.Semester.ToLower().Contains(searchQuery) ||
                    s.Email.ToLower().Contains(searchQuery) ||
                    s.Major.ToLower().Contains(searchQuery) ||
                    s.Email.ToLower().Contains(searchQuery));

                // Log the filtered query for debugging
                Console.WriteLine($"Filtered Query: {studentsQuery.ToQueryString()}");
            }

            // Execute the query and load the results into a list
            var students = await studentsQuery.ToListAsync();

            // Pass the search query back to the view to preserve the search term
            ViewData["CurrentFilter"] = searchQuery;

            // Return the view with the filtered students
            return View(students);
        }
        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,Phone,Email,Major,Year,Semester,Shift")] Student student)
        {
            
                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student created successfully!";
                return RedirectToAction(nameof(Index));
          
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Gender,Phone,Email,Major,Year,Semester,Shift")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
         
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Student not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}