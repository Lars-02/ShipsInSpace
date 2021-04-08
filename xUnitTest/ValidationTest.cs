using System.Collections.Generic;
using System.Linq;
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
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void CheckEnergyIsLessThenCapacity(int energyModifier)
        {
            var ship = ShipFactory.CreateShip();
            ship.Setup(s => s.Energy).Returns(50);

            var calculations = new Mock<Calculations>();
            
            calculations.Setup(c => c.GetEnergyConsumption(ship.Object)).Returns(50+energyModifier);
            var modelState = new ModelStateDictionary();
            Validation.ValidateShip(modelState, ship.Object, calculations.Object);
            var valid = !modelState.TryGetValue("EnergyConsumptionOverdraft", out _);

            if (energyModifier <= 0)
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

        [Theory]
        [InlineData(2, 9)]
        [InlineData(2, 1)]
        [InlineData(1, 9)]
        [InlineData(1, 1)]
        public void CheckImploderCombination(int engineId, int weaponId)
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            var weapon = WeaponFactory.CreateWeapon();
            var engine = EngineFactory.CreateEngine();
            
            wing.Setup(w => w.Hardpoint).Returns((new[] {weapon.Object}).ToList());
            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());
            ship.Setup(s => s.Engine).Returns(engine.Object);
            
            engine.Setup(e => e.Id).Returns(engineId);
            weapon.Setup(w => w.Id).Returns(weaponId);

            var modelState = new ModelStateDictionary();
            Validation.ValidateShip(modelState, ship.Object, _calculations);
            var valid = !modelState.TryGetValue("ImplosionDanger", out _);
            
            if (engineId == 2 && weaponId == 9)
                Assert.False(valid);
            else
                Assert.True(valid);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void CheckWeaponAmount(int weaponAmountModifier)
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            
            var weapons = (new[] {WeaponFactory.CreateWeapon().Object}).ToList();

            wing.Setup(w => w.NumberOfHardpoints).Returns(1 + weaponAmountModifier);
            wing.Setup(w => w.Hardpoint).Returns(weapons);

            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());
            
            var modelState = new ModelStateDictionary();
            Validation.ValidateShip(modelState, ship.Object, _calculations);
            var valid = !modelState.TryGetValue("WeaponOverload", out _);

            if (weaponAmountModifier < 0)
                Assert.False(valid);
            else
                Assert.True(valid);
        }
    }
}
