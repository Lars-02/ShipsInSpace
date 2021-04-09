using Data.Model;

namespace Web.Utils.Interfaces
{
    public interface ICalculations
    {
        double GetEnergyConsumption(Ship ship);
        double GetShipWeight(Ship ship);
    }
}