using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Data.Service;
using Moq;

namespace xUnitTest.Factories
{
    public static class ShipFactory
    {
        public static Mock<Ship> CreateShip()
        {
            var ship = new Mock<Ship>();

            ship.Setup(s => s.Id).Returns(0);
            ship.Setup(s => s.Name).Returns("TestShip");
            ship.Setup(s => s.Hull).Returns(HullFactory.CreateHull().Object);
            ship.Setup(s => s.Wings).Returns((new[] {WingFactory.CreateWing().Object, WingFactory.CreateWing().Object}).ToList());
            ship.Setup(s => s.Engine).Returns(EngineFactory.CreateEngine().Object);

            return ship;
        }
    }
}