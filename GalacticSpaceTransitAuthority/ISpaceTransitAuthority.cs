using System.Collections.Generic;

namespace GalacticSpaceTransitAuthority
{
    public interface ISpaceTransitAuthority
    {
        IEnumerable<Hull> GetHulls();
        IEnumerable<Weapon> GetWeapons();
        IEnumerable<Wing> GetWings();
        IEnumerable<Engine> GetEngines();
        double CheckActualHullCapacity(Hull hull);
        string RegisterShip(string JSONString);
    }
}
