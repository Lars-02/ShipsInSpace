using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;
using Web.ViewModels.RegisterShip;

namespace Web.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View(new RegisterViewModel());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult RegisterPirate()
        {
            throw new NotImplementedException();
        }
    }
}