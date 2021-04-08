namespace Data.Model
{
    public class Weapon
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DamageTypeEnum DamageType { get; set; }
        public virtual int EnergyDrain { get; set; }
        public virtual int Weight { get; set; }
    }

    public enum DamageTypeEnum
    {
        Kinetic,
        Heat,
        Cold,
        Statis,
        Gravity
    }
}