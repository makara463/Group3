using Microsoft.AspNetCore.Mvc;

namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
