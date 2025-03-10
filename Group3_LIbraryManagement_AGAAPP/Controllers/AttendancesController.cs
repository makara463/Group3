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
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Attendances.Include(a => a.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,AttendanceDate,TimeIn,TimeOut,Status")] Attendance attendance)
        {
            
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance created successfully!";
                return RedirectToAction(nameof(Index));
           
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", attendance.StudentId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,StudentId,AttendanceDate,TimeIn,TimeOut,Status")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(attendance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Attendance not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(string id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}