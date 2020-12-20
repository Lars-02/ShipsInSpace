using System;
using System.Collections.Generic;
using System.Text;

namespace GalacticSpaceTransitAuthority
{
    public class Wing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Energy { get; set; }
        public int Weight { get; set; }
        public List<Weapon> Hardpoint { get; set; }
        public int WeaponSlots { get; set; }
    }
}
