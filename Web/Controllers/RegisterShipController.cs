using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data.Model;
using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Utils;
using Web.Utils.Interfaces;
using Web.ViewModels;
using Web.ViewModels.RegisterShip;

namespace Web.Controllers
{
    public class RegisterShipController : Controller
    {
        private readonly ISpaceTransitAuthority _spaceTransitAuthority;
        private ICalculations _calculations;

        public RegisterShipController(ISpaceTransitAuthority spaceTransitAuthority, ICalculations calculations)
        {
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

            Validation.ValidateShip(ModelState, ship, _calculations, viewModel.MaximumTakeoffMass);

            if (ModelState.ErrorCount <= 0)
                return View("Overview", new OverviewViewModel
                {
                    Ship = ship,
                    Weight = _calculations.GetShipWeight(ship),
                    EnergyConsumption = _calculations.GetEnergyConsumption(ship),
                    MaximumTakeoffMass = viewModel.MaximumTakeoffMass
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