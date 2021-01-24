using System.Linq;
using Data.Model;
using Web.Utils;

namespace Web.ViewModels.RegisterShip
{
    public class OverviewViewModel
    {
        public Ship Ship;

        public double Weight => Calculations.GetShipWeight(Ship);
        public double EnergyConsumption => Calculations.GetEnergyConsumption(Ship.Wings.SelectMany(wing => wing.Hardpoint));
    }
}
