using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Utils
{
    public static class Validation
    {
        private static ModelStateDictionary _modelState;
        private static Ship _ship;
        
        public static void ValidateShip(ModelStateDictionary modelState, Ship ship)
        {
            _modelState = modelState;
            _ship = ship;
            ValidateNumberOfWeapons();
            ValidateHullWeight();
            ValidateEnergyConsumption();
            ValidateImploderWeapons();
        }
        
        private static void ValidateNumberOfWeapons()
        {
            foreach (var wing in _ship.Wings.Where(wing => wing.Hardpoint.Count > wing.NumberOfHardpoints))
                _modelState.AddModelError("WeaponOverload", "There are too many weapons on " + wing.Name);
        }

        private static void ValidateHullWeight()
        {
            if (Calculations.GetShipWeight(_ship) > (int) _ship.Hull.DefaultMaximumTakeOffMass)
                _modelState.AddModelError("CapacityOverload", "The ship is too heavy to take off");
        }

        private static void ValidateEnergyConsumption()
        {
            if (Calculations.GetEnergyConsumption(_ship.Wings.SelectMany(wing => wing.Hardpoint)) > _ship.Energy)
                _modelState.AddModelError("EnergyConsumptionOverdraft", "The energy consumption of the ship is too high");
        }

        private static void ValidateImploderWeapons()
        {
            if (_ship.Engine.Id == 2 && _ship.Wings.Any(wing => wing.Hardpoint.Any(weapon => weapon.Id == 9)))
                _modelState.AddModelError("ImplosionDanger", "The combination of Imploder weapon and Intrepid Class engine is not allowed");
        }
    }
}