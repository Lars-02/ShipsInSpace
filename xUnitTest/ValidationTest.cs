using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Web.Controllers;
using Web.Utils;
using Xunit;
using Xunit.Abstractions;
using xUnitTest.Factories;

namespace xUnitTest
{
    public class ValidShipTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly RegisterShipController _shipController;

        public ValidShipTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var spaceTransitAuthority = new SpaceTransitAuthority();
            var calculations = new Calculations();
            _shipController = new RegisterShipController(spaceTransitAuthority, calculations);
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
