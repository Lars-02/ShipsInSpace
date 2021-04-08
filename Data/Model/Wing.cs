using System.Collections.Generic;

namespace Data.Model
{
    public class Wing
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Agility { get; set; }
        public virtual int Speed { get; set; }
        public virtual int Energy { get; set; }
        public virtual int Weight { get; set; }
        public virtual List<Weapon> Hardpoint { get; set; }
        public virtual int NumberOfHardpoints { get; set; }
    }
}