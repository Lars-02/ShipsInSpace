using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Hull
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Agility { get; set; }
        public virtual int Speed { get; set; }
        public virtual int ColdShielding { get; set; }
        public virtual int HeatShielding { get; set; }
        public virtual TakeOffMassEnum DefaultMaximumTakeOffMass { get; set; }
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