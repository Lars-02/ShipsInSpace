using Data.Model;

namespace Web.ViewModels.RegisterShip
{
    public class OverviewViewModel : FullShipViewModel
    {
        public Ship Ship { get; set; }
        public double Weight { get; set; }
        public double EnergyConsumption { get; set; }
    }
}