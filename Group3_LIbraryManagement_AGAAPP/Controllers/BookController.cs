using Microsoft.AspNetCore.Mvc;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

    }
}
