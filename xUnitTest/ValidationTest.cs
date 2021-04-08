using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Web.Utils;
using Xunit;
using xUnitTest.Factories;

namespace xUnitTest
{
    public class ValidShipTest
    {
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
            var valid = !modelState.TryGetValue("CapacityOverload", out var x);

            if (weightModifier <= 0)
                Assert.True(valid);
            else
                Assert.False(valid);
        }
    }
}
