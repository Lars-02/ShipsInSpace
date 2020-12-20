using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data.Model;
using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models;
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
            return View(new RegisterShipViewModel
            {
                Engines = new List<SelectListItem>(_spaceTransitAuthority.GetEngines().Select(engine => new SelectListItem(engine.Name + " - " + engine.Energy, engine.Id.ToString()))),
                Hulls = new List<SelectListItem>(_spaceTransitAuthority.GetHulls().Select(hull => new SelectListItem(hull.Name + " - " + hull.DefaultMaximumTakeOffMass, hull.Id.ToString())))
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult SetupShip(RegisterShipViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", viewModel);

            return View("Wings", new RegisterShipWingsViewModel
            {
                NumberOfWings = viewModel.NumberOfWings,
                Engine = _spaceTransitAuthority.GetEngines().FirstOrDefault(engine => engine.Id == viewModel.SelectedEngine),
                Hull = _spaceTransitAuthority.GetHulls().FirstOrDefault(hull => hull.Id == viewModel.SelectedHull),
                Wings = new List<SelectListItem>(_spaceTransitAuthority.GetWings().Select(wing => new SelectListItem(wing.Name + " - " + wing.Energy + " - " + wing.Weight + " - " + wing.WeaponSlots, wing.Id.ToString())))
            });
        }

        public IActionResult AddWings(RegisterShipWingsViewModel viewModel)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyNumberOfWings(int numberOfWings)
        {
            return numberOfWings % 2 == 0 ? Json(true) : Json("A ship has an even number of wings.");
        }
    }
}