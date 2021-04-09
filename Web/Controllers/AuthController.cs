using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Data.Model;
using Web.ViewModels.Auth;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            CreateUsersAndRoles().GetAwaiter().GetResult();
        }

        private async Task<bool> CreateUsersAndRoles()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole {Name = Roles.Admin});
            
                var user = new IdentityUser
                {
                    UserName = "00-000-00",
                };
            
                var password = "admin";
            
                var result = await _userManager.CreateAsync(user, password);
            
                Console.WriteLine(result.Succeeded);
                
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
            }
            
            if (!await _roleManager.RoleExistsAsync(Roles.Pirate))
                await _roleManager.CreateAsync(new IdentityRole {Name = Roles.Pirate});

            return true;
        }
        
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult RegisterPirate()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
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
            await _userManager.AddToRoleAsync(user, Roles.Pirate);

            return View("Registered", viewModel);
        }

        [HttpGet]
        public IActionResult Login()
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