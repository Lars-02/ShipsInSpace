namespace Data.Model
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DamageTypeEnum DamageType { get; set; }
        public int EnergyDrain { get; set; }
        public int Weight { get; set; }
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
