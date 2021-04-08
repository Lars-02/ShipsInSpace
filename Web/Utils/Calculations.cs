using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Web.Utils.Interfaces;

namespace Web.Utils
{
    public class Calculations : ICalculations
    {
        public virtual double GetEnergyConsumption(IEnumerable<Weapon> weapons)
        {
            var totalEnergyConsumption = 0.0;
            foreach (var damageType in Enum.GetValues(typeof(DamageTypeEnum)).Cast<DamageTypeEnum>())
            {
                var numberOfWeapons = 0;
                var typeEnergyConsumption = 0.0;
                foreach (var weapon in weapons)
                {
                    if (weapon.DamageType != damageType) continue;
                    numberOfWeapons++;
                    typeEnergyConsumption += weapon.EnergyDrain;
                }

                if (numberOfWeapons >= 3)
                    typeEnergyConsumption *= 0.8;
                totalEnergyConsumption += typeEnergyConsumption;
            }

            return totalEnergyConsumption;
        }

        public virtual double GetShipWeight(Ship ship)
        {
            return (ship.Engine.Weight + GetWingsWeight(ship.Wings)) *
                   (HasTwoStatisWeapons(ship.Wings.SelectMany(wing => wing.Hardpoint)) ? 0.85 : 1);
        }

        private static int GetWingsWeight(IEnumerable<Wing> wings)
        {
            return wings.Sum(wing => wing.Weight + GetWeaponsWeight(wing.Hardpoint));
        }

        private static int GetWeaponsWeight(IEnumerable<Weapon> weapons)
        {
            return weapons.Sum(weapon => weapon.Weight);
        }

        private bool HasTwoStatisWeapons(IEnumerable<Weapon> weapons)
        {
            return weapons.Count(weapon => weapon.DamageType == DamageTypeEnum.Statis) >= 2;
        }
    }
}