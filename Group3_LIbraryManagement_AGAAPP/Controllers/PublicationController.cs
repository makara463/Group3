using Microsoft.AspNetCore.Mvc;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class PublicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
