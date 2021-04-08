using Data.Model;
using Moq;

namespace xUnitTest.Factories
{
    public static class WeaponFactory
    {
        public static Mock<Weapon> CreateWeapon()
        {
            var weapon = new Mock<Weapon>();

            weapon.Setup(w => w.Id).Returns(0);
            weapon.Setup(w => w.Name).Returns("TestWeapon");
            weapon.Setup(w => w.DamageType).Returns(DamageTypeEnum.Kinetic);
            weapon.Setup(w => w.EnergyDrain).Returns(5);
            weapon.Setup(w => w.Weight).Returns(5);
            
            return weapon;
        }
    }
}