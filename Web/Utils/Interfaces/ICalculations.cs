using System.Collections.Generic;
using Data.Model;

namespace Web.Utils.Interfaces
{
    public interface ICalculations
    {
        double GetEnergyConsumption(IEnumerable<Weapon> weapons);
        double GetShipWeight(Ship ship);
    }
}