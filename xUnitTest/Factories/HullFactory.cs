using Data.Model;
using Moq;

namespace xUnitTest.Factories
{
    public static class HullFactory
    {
        public static Mock<Hull> CreateHull()
        {
            var hull = new Mock<Hull>();

            hull.Setup(h => h.Id).Returns(0);
            hull.Setup(h => h.Name).Returns("TestHull");
            hull.Setup(h => h.Agility).Returns(5);
            hull.Setup(h => h.Speed).Returns(5);
            hull.Setup(h => h.ColdShielding).Returns(5);
            hull.Setup(h => h.HeatShielding).Returns(5);
            hull.Setup(h => h.DefaultMaximumTakeOffMass).Returns(TakeOffMassEnum.Interceptor);
            
            return hull;
        }
    }
}