using System.Collections.Generic;
using System.Linq;
using Data.Model;

namespace Web.Utils
{
    public static class Calculations
    {
        public static double GetShipWeight(Ship ship)
        {
            return (ship.Engine.Weight + GetWingsWeight(ship.Wings)) * (HasTwoStatisWeapons(ship.Wings.SelectMany(wing => wing.Hardpoint)) ? 0.85 : 1);
        }

        private static int GetWingsWeight(IEnumerable<Wing> wings) =>
            wings.Sum(wing => wing.Weight + GetWeaponsWeight(wing.Hardpoint));

        private static int GetWeaponsWeight(IEnumerable<Weapon> weapons) => weapons.Sum(weapon => weapon.Weight);

        private static bool HasTwoStatisWeapons(IEnumerable<Weapon> weapons) =>
            weapons.Count(weapon => weapon.DamageType == DamageTypeEnum.Statis) >= 2;
    }
}