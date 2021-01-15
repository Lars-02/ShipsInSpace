using System;
using System.Collections.Generic;
using System.Linq;
using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.RegisterShip;
using System.Diagnostics;
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

            var wings = new List<WingViewModel>();
            
            for (var i = 0; i < viewModel.NumberOfWings; i++)
                wings.Add(new WingViewModel());
            
            return View("Wings", new WingsViewModel
            {
                SelectedWings = wings,
                AvailableWings = _spaceTransitAuthority.GetWings().Select(wing => new WingViewModel
                    {Id = wing.Id, Name = wing.Name, WeaponSlots = wing.NumberOfHardpoints, Weight = wing.Weight}),
                Engine = _spaceTransitAuthority.GetEngines()
                    .FirstOrDefault(engine => engine.Id == viewModel.SelectedEngine),
                Hull = _spaceTransitAuthority.GetHulls().FirstOrDefault(hull => hull.Id == viewModel.SelectedHull)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyNumberOfWings(int numberOfWings)
        {
            return numberOfWings % 2 == 0 ? Json(true) : Json("A ship has an even number of wings.");
        }

        [HttpPost]
        public IActionResult Wings(WingsViewModel wingsViewModel)
        {
            Console.WriteLine(wingsViewModel);
            Console.WriteLine(wingsViewModel.Hull);
            Console.WriteLine(wingsViewModel.Engine);
            Console.WriteLine(wingsViewModel.SelectedWings);
            Console.WriteLine(wingsViewModel.SelectedWings.Count);
            foreach (var x in wingsViewModel.SelectedWings)
                Console.WriteLine(x.Id);

            return View(wingsViewModel);
        }
    }
}
