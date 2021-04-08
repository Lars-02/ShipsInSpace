using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Web.Controllers;
using Web.Utils;
using Xunit;
using Xunit.Abstractions;

namespace xUnitTest
{
    public class ValidShipTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly RegisterShipController _shipController;
        private SpaceTransitAuthority _spaceTransitAuthority;

        public ValidShipTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _spaceTransitAuthority = new SpaceTransitAuthority();
            var calculations = new Calculations();
            _shipController = new RegisterShipController(_spaceTransitAuthority, calculations);
        }

        private Mock<Ship> CreateShip()
        {
            var ship = new Mock<Ship>();

            var wings = _spaceTransitAuthority.GetWings().Take(2).ToList();
            wings.ForEach(w => w.Hardpoint = new List<Weapon>());
            wings.ForEach(w => w.Hardpoint.Add(_spaceTransitAuthority.GetWeapons().First(wpn => wpn.Id == 4)));
            
            ship.Setup(s => s.Id).Returns(0);
            ship.Setup(s => s.Name).Returns("TestShip");
            ship.Setup(s => s.Hull).Returns(_spaceTransitAuthority.GetHulls().First());
            ship.Setup(s => s.Wings).Returns(wings);
            ship.Setup(s => s.Engine).Returns(_spaceTransitAuthority.GetEngines().First());

            return ship;
        }
            
        [Fact]
        public void CheckEvenNumberOfWingsFrontend()
        {
            for (var i = 1; i < 10; i++)
            {
                var jsonResult = _shipController.VerifyNumberOfWings(i) as JsonResult;
                
                if (i % 2 == 0)
                    Assert.Equal("True", jsonResult!.Value.ToString());
                else
                    Assert.NotEqual("True", jsonResult!.Value.ToString());
            }
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void CheckMassIsLessThenCapacity(int weightModifier)
        {
            var ship = CreateShip();

            var calculations = new Mock<Calculations>();
            
            calculations.Setup(c => c.GetShipWeight(ship.Object)).Returns((int) ship.Object.Hull.DefaultMaximumTakeOffMass+weightModifier);
            var modelState = new ModelStateDictionary();
            Validation.ValidateShip(modelState, ship.Object, calculations.Object);
            var valid = !modelState.TryGetValue("CapacityOverload", out var x);

            if (weightModifier <= 0)
                Assert.True(valid);
            else
                Assert.False(valid);
        }
    }
}
