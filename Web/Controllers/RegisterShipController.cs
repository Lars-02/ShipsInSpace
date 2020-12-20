using System;
using System.Diagnostics;
using System.Linq;
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
                Engines = new SelectList(_spaceTransitAuthority.GetEngines().Select(engine => new SelectListItem(engine.Name + " - " + engine.Energy, engine.Id.ToString()))),
                Hulls = new SelectList(_spaceTransitAuthority.GetHulls().Select(hull => new SelectListItem(hull.Name + " - " + hull.DefaultMaximumTakeOffMass, hull.Id.ToString())))
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult RegisterPirate()
        {
            throw new NotImplementedException();
        }
    }
}