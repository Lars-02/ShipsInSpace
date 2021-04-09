using System;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.RegisterShip;

namespace Web.Controllers
{
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult RegisterPirate()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult RegisterPirate(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            viewModel.License = (Licence) viewModel.LicenceId;
            viewModel.SecretCode = Guid.NewGuid().ToString().Substring(0, 8);

            return View("Registered", viewModel);
        }
    }
}