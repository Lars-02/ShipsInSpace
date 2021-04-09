using Data.Service;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;
using Web.Utils;
using Xunit;

namespace xUnitTest
{
    public class ShipControllerTest
    {
        private readonly RegisterShipController _shipController;

        public ShipControllerTest()
        {
            var spaceTransitAuthority = new SpaceTransitAuthority();
            var calculations = new Calculations();
            _shipController = new RegisterShipController(null, null, spaceTransitAuthority, calculations);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void CheckEvenNumberOfWingsFrontend(int amount)
        {
            var jsonResult = _shipController.VerifyNumberOfWings(amount) as JsonResult;
            
            if (amount % 2 == 0)
                Assert.Equal("True", jsonResult!.Value.ToString());
            else
                Assert.NotEqual("True", jsonResult!.Value.ToString());
        }
    }
}