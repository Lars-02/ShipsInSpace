using System.Collections.Generic;
using System.Linq;
using Data.Model;

namespace Web.ViewModels.RegisterShip
{
    public class OverviewViewModel
    {
        public Hull Hull { get; set; }
        public Engine Engine { get; set; }
        public IEnumerable<Wing> Wings { get; set; }

        public long TotalWeight => Engine.Weight + Wings.Sum(wing => wing.Weight + wing.Hardpoint.Sum(weapon => weapon.Weight));

        public long TotalEnergyUsage => Wings.Sum(wing => wing.Hardpoint.Sum(weapon => weapon.EnergyDrain));
        public long TotalAgility => Wings.Sum(wing => wing.Agility);
        public long TotalSpeed => Wings.Sum(wing => wing.Speed);
    }
}
