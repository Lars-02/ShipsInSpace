using System;
using System.Threading.Tasks;
using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.Auth;

namespace Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult RegisterPirate()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPirate(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            viewModel.LicensePlate = viewModel.LicensePlate.ToUpper();
            viewModel.License = (Licence) viewModel.LicenceId;
            viewModel.SecretCode = Guid.NewGuid().ToString().Substring(0, 8);

            var user = new IdentityUser
            {
                UserName = viewModel.LicensePlate,
            };

            var result = await _userManager.CreateAsync(user, viewModel.SecretCode);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return View(viewModel);
            }

            return View("Registered", viewModel);
        }
    }
}