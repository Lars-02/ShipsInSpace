using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Data.Model;
using Data.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Data.Model;
using Web.Utils;
using Web.Utils.Interfaces;
using Web.ViewModels;
using Web.ViewModels.RegisterShip;

namespace Web.Controllers
{
    [Authorize(Roles = Roles.Pirate)]
    public class RegisterShipController : Controller
    {
        private readonly ISpaceTransitAuthority _spaceTransitAuthority;
        private readonly ICalculations _calculations;
        private readonly UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public RegisterShipController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ISpaceTransitAuthority spaceTransitAuthority, ICalculations calculations)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _spaceTransitAuthority = spaceTransitAuthority;
            _calculations = calculations;
        }

        public IActionResult Index()
        {
            return RedirectToAction("SetupShip");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        public IActionResult SetupShip()
        {
            return View(new RegisterShipViewModel
            {
                Engines = new List<SelectListItem>(_spaceTransitAuthority.GetEngines().Select(engine =>
                    new SelectListItem(engine.Name + " - " + engine.Energy, engine.Id.ToString()))),
                Hulls = new List<SelectListItem>(_spaceTransitAuthority.GetHulls().Select(hull =>
                    new SelectListItem(hull.Name + " - " + hull.DefaultMaximumTakeOffMass, hull.Id.ToString())))
            });
        }

        [HttpPost]
        public IActionResult SetupShip(RegisterShipViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return View("Wings", new WingsViewModel
            {
                SelectedWings = new int[viewModel.NumberOfWings],
                SelectedWeapons = new List<int>[viewModel.NumberOfWings],
                AvailableWings = _spaceTransitAuthority.GetWings(),
                AvailableWeapons = _spaceTransitAuthority.GetWeapons(),
                EngineId = viewModel.SelectedEngine,
                HullId = viewModel.SelectedHull,
                MaximumTakeoffMass = _spaceTransitAuthority.CheckActualHullCapacity(_spaceTransitAuthority
                    .GetHulls().FirstOrDefault(h => h.Id == viewModel.SelectedHull))
            });
        }

        [HttpPost]
        public IActionResult Wings(WingsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AvailableWings = _spaceTransitAuthority.GetWings();
                viewModel.AvailableWeapons = _spaceTransitAuthority.GetWeapons();
                return View(viewModel);
            }

            var ship = CreateShip(viewModel);

            var license = (Licence) int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "License")?.Value ?? string.Empty);
            Validation.ValidateShip(ModelState, ship, _calculations, viewModel.MaximumTakeoffMass, license);

            if (ModelState.ErrorCount > 0)
            {
                viewModel.AvailableWings = _spaceTransitAuthority.GetWings();
                viewModel.AvailableWeapons = _spaceTransitAuthority.GetWeapons();
                return View("Wings", viewModel);
            }

            return View("Overview", new OverviewViewModel
            {
                Ship = ship,
                Weight = _calculations.GetShipWeight(ship),
                EnergyConsumption = _calculations.GetEnergyConsumption(ship),
                MaximumTakeoffMass = viewModel.MaximumTakeoffMass,
                HullId = viewModel.HullId,
                EngineId = viewModel.EngineId,
                SelectedWings = viewModel.SelectedWings,
                SelectedWeapons = viewModel.SelectedWeapons,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(OverviewViewModel viewModel)
        {
            var ship = CreateShip(viewModel);
            var license = (Licence) int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "License")?.Value ?? string.Empty);
            Validation.ValidateShip(ModelState, ship, _calculations, viewModel.MaximumTakeoffMass, license);

            var shipJson = JsonSerializer.Serialize(ship);
            var registrationId = _spaceTransitAuthority.RegisterShip(shipJson);

            if (ModelState.ErrorCount > 0 || registrationId.Length == 0)
                return Json("Invalid ship, don't mess with the variables!!");

            var name = User.Identity?.Name;

            await _signInManager.SignOutAsync();
            await _userManager.SetLockoutEndDateAsync(await _userManager.FindByNameAsync(name), DateTime.MaxValue);

            return RedirectToAction("Confirmation", "RegisterShip", new ConfirmationViewModel
            {
                TransponderCode = registrationId
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Confirmation(ConfirmationViewModel viewModel)
        {
            if (viewModel.TransponderCode == null)
                return RedirectToAction("Index", "Home");
            
            return View(viewModel);
        }

        private Ship CreateShip(FullShipViewModel viewModel)
        {
            var ship = new Ship
            {
                Wings = new List<Wing>(),
                Engine = _spaceTransitAuthority.GetEngines().FirstOrDefault(engine => engine.Id == viewModel.EngineId),
                Hull = _spaceTransitAuthority.GetHulls().FirstOrDefault(hull => hull.Id == viewModel.HullId)
            };

            for (var i = 0; i < viewModel.SelectedWings.Length; i++)
            {
                var wing = _spaceTransitAuthority.GetWings()
                    .FirstOrDefault(selectedWing => selectedWing.Id == viewModel.SelectedWings[i]);

                var newWing = new Wing
                {
                    Agility = wing.Agility,
                    Energy = wing.Energy,
                    Hardpoint = new List<Weapon>(),
                    Id = wing.Id,
                    Name = wing.Name,
                    Speed = wing.Speed,
                    Weight = wing.Weight,
                    NumberOfHardpoints = wing.NumberOfHardpoints
                };


                if (viewModel.SelectedWeapons != null && i < viewModel.SelectedWeapons.Length &&
                    viewModel.SelectedWeapons[i] != null)
                {
                    var weapons = _spaceTransitAuthority.GetWeapons()
                        .Where(weapon => viewModel.SelectedWeapons[i].Contains(weapon.Id));

                    foreach (var weapon in weapons)
                        newWing.Hardpoint.Add(weapon);
                }

                ship.Wings.Add(newWing);
            }

            return ship;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyNumberOfWings(int numberOfWings)
        {
            return numberOfWings % 2 == 0 ? Json(true) : Json("A ship has an even number of wings.");
        }
    }
}