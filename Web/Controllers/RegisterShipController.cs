using System;
using System.Collections.Generic;
using System.Linq;
using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.RegisterShip;
using System.Diagnostics;
using Data.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModels;

namespace Web.Controllers
{
    public class RegisterShipController : Controller
    {
        private readonly ISpaceTransitAuthority _spaceTransitAuthority;
        private readonly IEnumerable<Wing> _availableWings;
        private readonly IEnumerable<Weapon> _availableWeapons;

        public RegisterShipController(ISpaceTransitAuthority spaceTransitAuthority)
        {
            _spaceTransitAuthority = spaceTransitAuthority;

            _availableWings = _spaceTransitAuthority.GetWings();
            _availableWeapons = _spaceTransitAuthority.GetWeapons();
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
                AvailableWings = _availableWings,
                AvailableWeapons = _availableWeapons,
                EngineId = viewModel.SelectedEngine,
                HullId = viewModel.SelectedHull
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyNumberOfWings(int numberOfWings)
        {
            return numberOfWings % 2 == 0 ? Json(true) : Json("A ship has an even number of wings.");
        }

        [HttpPost]
        public IActionResult Wings(WingsViewModel viewModel)
        {
            viewModel.AvailableWings = _availableWings;
            viewModel.AvailableWeapons = _availableWeapons;

            if (!ModelState.IsValid)
                return View(viewModel);

            var wings = new List<Wing>();

            for (var i = 0; i < viewModel.SelectedWings.Length; i++)
            {
                var weapons = _spaceTransitAuthority.GetWeapons()
                    .Where(weapon => viewModel.SelectedWeapons[i].Contains(weapon.Id));

                var wing = _spaceTransitAuthority.GetWings().FirstOrDefault(wing => wing.Id == viewModel.SelectedWings[i]);

                wing!.Hardpoint = weapons.ToList();
                wings.Add(wing);
            }
            
            return View("Overview", new OverviewViewModel
            {
                Hull = _spaceTransitAuthority.GetHulls().FirstOrDefault(hull => hull.Id == viewModel.HullId),
                Engine = _spaceTransitAuthority.GetEngines().FirstOrDefault(engine => engine.Id == viewModel.EngineId),
                Wings = wings
            });
        }
    }
}
