using System;
using System.Collections.Generic;
using System.Linq;
using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.RegisterShip;

namespace Web.Controllers
{
    public class RegisterShipController : Controller
    {
        private ISpaceTransitAuthority _spaceTransitAuthority;

        public RegisterShipController(ISpaceTransitAuthority spaceTransitAuthority)
        {
            _spaceTransitAuthority = spaceTransitAuthority;
        }

        public IActionResult Wings()
        {
            var wings = new List<WingViewModel>();
            for (var i = 0; i < 5; i++)
                wings.Add(new WingViewModel());
            return View(new WingsViewModel
            {
                SelectedWings = wings,
                AvailableWings = _spaceTransitAuthority.GetWings().Select(wing => new WingViewModel{Id = wing.Id, Name = wing.Name, WeaponSlots = wing.NumberOfHardpoints, Weight = wing.Weight})
            });
        }

        [HttpPost]
        public IActionResult Wings(WingsViewModel wingsViewModel)
        {
            Console.WriteLine(wingsViewModel);
            Console.WriteLine(wingsViewModel.SelectedWings);
            Console.WriteLine(wingsViewModel.SelectedWings.Count);
            foreach (var x in wingsViewModel.SelectedWings)
                Console.WriteLine(x.Id);

            return RedirectToAction("Wings");
        }
    }
}
