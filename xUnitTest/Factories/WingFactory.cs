using System.Linq;
using Data.Model;
using Moq;

namespace xUnitTest.Factories
{
    public static class WingFactory
    {
        public static Mock<Wing> CreateWing()
        {
            var wing = new Mock<Wing>();

            wing.Setup(w => w.Id).Returns(0);
            wing.Setup(w => w.Name).Returns("TestWing");
            wing.Setup(w => w.Agility).Returns(5);
            wing.Setup(w => w.Speed).Returns(5);
            wing.Setup(w => w.Energy).Returns(5);
            wing.Setup(w => w.Weight).Returns(5);
            wing.Setup(w => w.Hardpoint).Returns((new[] {WeaponFactory.CreateWeapon().Object, WeaponFactory.CreateWeapon().Object}).ToList());
            wing.Setup(w => w.NumberOfHardpoints).Returns(5);

            return wing;
        }
    }
}