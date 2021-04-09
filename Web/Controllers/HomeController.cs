using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Data.Model;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction("RegisterPirate", "Auth");
            if (User.IsInRole(Roles.Pirate))
                return RedirectToAction("Index", "RegisterShip");
            
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}