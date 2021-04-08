using Data.Model;

namespace Web.ViewModels.RegisterShip
{
    public class OverviewViewModel
    {
        public Ship Ship { get; init; }
        public double Weight { get; init; }
        public double EnergyConsumption { get; init; }
    }
}