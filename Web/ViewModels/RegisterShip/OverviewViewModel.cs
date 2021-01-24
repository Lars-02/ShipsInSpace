using Data.Model;
using Web.Utils;

namespace Web.ViewModels.RegisterShip
{
    public class OverviewViewModel
    {
        public Ship Ship;

        public double Weight => Calculations.GetShipWeight(Ship);
    }
}
