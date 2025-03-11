using Group3_LIbraryManagement_AGAAPP.Data;
using Microsoft.AspNetCore.Mvc;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }
    }
}
