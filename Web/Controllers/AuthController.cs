using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.Auth;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            
            await _userManager.AddClaimAsync(user, new Claim("License", viewModel.LicenceId.ToString()));

            return View("Registered", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(new LoginViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            
            var result = await _signInManager.PasswordSignInAsync(viewModel.LicensePlate, viewModel.SecretCode, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}