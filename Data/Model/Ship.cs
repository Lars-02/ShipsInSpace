using System.Collections.Generic;
using System.Linq;

namespace Data.Model
{
    public class Ship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Hull Hull { get; set; }
        public List<Wing> Wings { get; set; }
        public Engine Engine { get; set; }
        public int Agility => Hull?.Agility + Wings?.Sum(w => w.Agility) ?? 0;
        public int Speed => Hull?.Speed + Wings?.Sum(w => w.Speed) ?? 0;
        public int Energy => Engine?.Energy + Wings?.Sum(w => w.Energy) ?? 0;
    }
}