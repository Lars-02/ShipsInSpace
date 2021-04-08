using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Hull
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Agility { get; set; }
        public int Speed { get; set; }
        public int ColdShielding { get; set; }
        public int HeatShielding { get; set; }
        public TakeOffMassEnum DefaultMaximumTakeOffMass { get; set; }
    }

    public enum TakeOffMassEnum
    {
        [Display(Name = "Interceptor")] Interceptor = 600,
        [Display(Name = "Light Fighter")] LightFighter = 950,
        [Display(Name = "Fighter")] MediumFighter = 1000,
        [Display(Name = "Tank")] Tank = 1400,
        [Display(Name = "Capital Ship")] HeavyTank = 2000
    }
}