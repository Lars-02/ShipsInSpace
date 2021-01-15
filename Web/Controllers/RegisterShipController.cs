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

        public RegisterShipController(ISpaceTransitAuthority spaceTransitAuthority)
        {
            _spaceTransitAuthority = spaceTransitAuthority;
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
                HullId = viewModel.SelectedHull
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
            
            ship.Wings.ForEach(wing => Console.WriteLine(wing.Name + " " + wing.Hardpoint.Count + " " + wing.NumberOfHardpoints));
            foreach (var wing in ship.Wings.Where(wing => wing.Hardpoint.Count > wing.NumberOfHardpoints))
                ModelState.AddModelError("WeaponOverload", "There are too many weapons on " + wing.Name);

            if (ModelState.ErrorCount <= 0)
                return View("Overview", new OverviewViewModel
                {
                    Hull = ship.Hull,
                    Engine = ship.Engine,
                    Wings = ship.Wings
                });
            
            viewModel.AvailableWings = _spaceTransitAuthority.GetWings();
            viewModel.AvailableWeapons = _spaceTransitAuthority.GetWeapons();
            return View("Wings", viewModel);
        }

        private Ship CreateShip(WingsViewModel viewModel)
        {
            var ship = new Ship
            {
                Wings = new List<Wing>(),
                Engine = _spaceTransitAuthority.GetEngines().FirstOrDefault(engine => engine.Id == viewModel.EngineId),
                Hull = _spaceTransitAuthority.GetHulls().FirstOrDefault(hull => hull.Id == viewModel.HullId),
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

                var weapons = _spaceTransitAuthority.GetWeapons()
                    .Where(weapon => viewModel.SelectedWeapons[i].Contains(weapon.Id));

                foreach (var weapon in weapons)
                    newWing.Hardpoint.Add(weapon);

                ship.Wings.Add(newWing);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyNumberOfWings(int numberOfWings)
        {
            return numberOfWings % 2 == 0 ? Json(true) : Json("A ship has an even number of wings.");
        }
    }
}