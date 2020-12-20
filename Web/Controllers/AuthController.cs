using Microsoft.AspNetCore.Mvc;
using Web.Models.Auth;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
    }
}
