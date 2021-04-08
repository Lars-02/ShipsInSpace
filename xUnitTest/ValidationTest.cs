using System.Collections.Generic;
using Data.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Web.Utils;
using Xunit;
using xUnitTest.Factories;

namespace xUnitTest
{
    public class ValidShipTest
    {
        private readonly Calculations _calculations;

        public ValidShipTest()
        {
            _calculations = new Calculations();
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void CheckMassIsLessThenCapacity(int weightModifier)
        {
            var ship = ShipFactory.CreateShip();

            var calculations = new Mock<Calculations>();
            
            calculations.Setup(c => c.GetShipWeight(ship.Object)).Returns((int) ship.Object.Hull.DefaultMaximumTakeOffMass+weightModifier);
            var modelState = new ModelStateDictionary();
            Validation.ValidateShip(modelState, ship.Object, calculations.Object);
            var valid = !modelState.TryGetValue("CapacityOverload", out _);

            if (weightModifier <= 0)
                Assert.True(valid);
            else
                Assert.False(valid);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void CheckEvenNumberOfWings(int amount)
        {
            var ship = ShipFactory.CreateShip();
            var wings = new List<Wing>();

            for (var i = 0; i < amount; i++)
                wings.Add(WingFactory.CreateWing().Object);

            ship.Setup(s => s.Wings).Returns(wings);

            var modelState = new ModelStateDictionary();
            
            Validation.ValidateShip(modelState, ship.Object, _calculations);
            
            var valid = !modelState.TryGetValue("OddWings", out _);
            
            if (amount % 2 == 0)
                Assert.True(valid);
            else
                Assert.False(valid);
        }
    }
}
