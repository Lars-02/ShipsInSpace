using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.Auth;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View("Index", new LoginViewModel());
        }
    }
}