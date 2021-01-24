using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Utils
{
    public static class Validation
    {
        private static ModelStateDictionary _modelState;
        
        public static void ValidateShip(ModelStateDictionary modelState, Ship ship)
        {
            _modelState = modelState;
            ValidateNumberOfWeapons(modelState, ship.Wings);
            ValidateHullWeight(modelState, ship);
        }
        
        private static void ValidateNumberOfWeapons(ModelStateDictionary modelState, IEnumerable<Wing> wings)
        {
            foreach (var wing in wings.Where(wing => wing.Hardpoint.Count > wing.NumberOfHardpoints))
                _modelState.AddModelError("WeaponOverload", "There are too many weapons on " + wing.Name);
        }

        private static void ValidateHullWeight(ModelStateDictionary modelState, Ship ship)
        {
            if (Calculations.GetShipWeight(ship) > (int) ship.Hull.DefaultMaximumTakeOffMass)
                _modelState.AddModelError("CapacityOverload", "The ship is too heavy to take off");
        }
    }
}