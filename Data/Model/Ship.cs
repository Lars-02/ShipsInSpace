using System.Collections.Generic;
using System.Linq;

namespace Data.Model
{
    public class Ship
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Hull Hull { get; set; }
        public virtual List<Wing> Wings { get; set; }
        public virtual Engine Engine { get; set; }
        public virtual int Agility => Hull?.Agility + Wings?.Sum(w => w.Agility) ?? 0;
        public virtual int Speed => Hull?.Speed + Wings?.Sum(w => w.Speed) ?? 0;
        public virtual int Energy => Engine?.Energy + Wings?.Sum(w => w.Energy) ?? 0;
    }
}