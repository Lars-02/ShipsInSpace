using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Web.Utils;
using Web.Utils.Interfaces;
using Xunit;
using xUnitTest.Factories;

namespace xUnitTest
{
    public class ValidShipTest
    {
        private readonly Mock<Calculations> _calculations;

        public ValidShipTest()
        {
            _calculations = new Mock<Calculations>();
        }

        private bool Validate(IMock<Ship> ship, string errorMessage)
        {
            return Validate(ship, errorMessage, _calculations);
        }

        private static bool Validate(IMock<Ship> ship, string errorMessage, IMock<ICalculations> calculations, double maximumTakeoffMass=0, Licence licence=Licence.Z)
        {
            var modelState = new ModelStateDictionary();
            Validation.ValidateShip(modelState, ship.Object, calculations.Object, maximumTakeoffMass, licence);
            var valid = !modelState.TryGetValue(errorMessage, out _);

            return valid;
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void CheckMassIsLessThenCapacity(int weightModifier)
        {
            var ship = ShipFactory.CreateShip();

            var calculations = new Mock<Calculations>();

            calculations.Setup(c => c.GetShipWeight(ship.Object)).Returns(50+weightModifier);
            
            var valid = Validate(ship, "CapacityOverload", calculations, 50);

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

            var valid = Validate(ship, "EnergyConsumptionOverdraft", calculations);

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

            var valid = Validate(ship, "OddWings");
            
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

            var valid = Validate(ship, "ImplosionDanger");
            
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

            var valid = Validate(ship, "WeaponOverload");

            if (weaponAmountModifier < 0)
                Assert.False(valid);
            else
                Assert.True(valid);
        }
        
        [Theory]
        [InlineData(DamageTypeEnum.Heat, DamageTypeEnum.Cold)]
        [InlineData(DamageTypeEnum.Heat, DamageTypeEnum.Heat)]
        [InlineData(DamageTypeEnum.Cold, DamageTypeEnum.Cold)]
        public void CheckHeatColdWeaponsNotAllowed(DamageTypeEnum weapon1Type, DamageTypeEnum weapon2Type)
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            
            var weapon1 = WeaponFactory.CreateWeapon();
            weapon1.Setup(w => w.DamageType).Returns(weapon1Type);
            var weapon2 = WeaponFactory.CreateWeapon();
            weapon2.Setup(w => w.DamageType).Returns(weapon2Type);

            wing.Setup(w => w.NumberOfHardpoints).Returns(2);
            wing.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object, weapon2.Object}).ToList());

            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());

            var valid = Validate(ship, "HeatStress");

            if ((weapon1Type == DamageTypeEnum.Heat || weapon2Type == DamageTypeEnum.Heat) && (weapon1Type == DamageTypeEnum.Cold || weapon2Type == DamageTypeEnum.Cold) && weapon1Type != weapon2Type)
                Assert.False(valid);
            else
                Assert.True(valid);
        }
        
        [Theory]
        [InlineData(DamageTypeEnum.Statis, DamageTypeEnum.Gravity)]
        [InlineData(DamageTypeEnum.Statis, DamageTypeEnum.Statis)]
        [InlineData(DamageTypeEnum.Gravity, DamageTypeEnum.Gravity)]
        public void CheckStatisGravityNotAllowed(DamageTypeEnum weapon1Type, DamageTypeEnum weapon2Type)
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            
            var weapon1 = WeaponFactory.CreateWeapon();
            weapon1.Setup(w => w.DamageType).Returns(weapon1Type);
            var weapon2 = WeaponFactory.CreateWeapon();
            weapon2.Setup(w => w.DamageType).Returns(weapon2Type);

            wing.Setup(w => w.NumberOfHardpoints).Returns(2);
            wing.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object, weapon2.Object}).ToList());

            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());

            var valid = Validate(ship, "ForceStress");

            if ((weapon1Type == DamageTypeEnum.Statis || weapon2Type == DamageTypeEnum.Statis) && (weapon1Type == DamageTypeEnum.Gravity || weapon2Type == DamageTypeEnum.Gravity) && weapon1Type != weapon2Type)
                Assert.False(valid);
            else
                Assert.True(valid);
        }
        
        [Theory]
        [InlineData(33)]
        [InlineData(34)]
        [InlineData(35)]
        [InlineData(-33)]
        [InlineData(-34)]
        [InlineData(-35)]
        public void CheckEnergyDifferenceLessThen35(int energyDifference)
        {
            var ship = ShipFactory.CreateShip();
            var wing1 = WingFactory.CreateWing();
            var wing2 = WingFactory.CreateWing();
            
            var weapon1 = WeaponFactory.CreateWeapon();
            weapon1.Setup(w => w.DamageType).Returns(DamageTypeEnum.Kinetic);
            weapon1.Setup(w => w.EnergyDrain).Returns(50 + energyDifference);
            var weapon2 = WeaponFactory.CreateWeapon();
            weapon2.Setup(w => w.DamageType).Returns(DamageTypeEnum.Kinetic);
            weapon2.Setup(w => w.EnergyDrain).Returns(50);
            
            var weapon3 = WeaponFactory.CreateWeapon();
            weapon3.Setup(w => w.DamageType).Returns(DamageTypeEnum.Kinetic);
            weapon3.Setup(w => w.EnergyDrain).Returns(100);
            
            var weapon4 = WeaponFactory.CreateWeapon();
            weapon4.Setup(w => w.DamageType).Returns(DamageTypeEnum.Cold);
            weapon4.Setup(w => w.EnergyDrain).Returns(100);

            wing1.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object, weapon2.Object}).ToList());
            wing2.Setup(w => w.Hardpoint).Returns((new[] {weapon3.Object, weapon4.Object}).ToList());

            ship.Setup(s => s.Wings).Returns((new[] {wing1.Object, wing2.Object}).ToList());

            var valid = Validate(ship, "KineticDifference");

            if (Math.Abs(energyDifference) < 35)
                Assert.True(valid);
            else
                Assert.False(valid);
        }

        [Fact]
        public void CheckKineticEnergyOnlyOneWingLessThen35()
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            
            var weapon = WeaponFactory.CreateWeapon();
            weapon.Setup(w => w.DamageType).Returns(DamageTypeEnum.Kinetic);
            
            wing.Setup(w => w.Hardpoint).Returns((new[] {weapon.Object}).ToList());
            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());

            weapon.Setup(w => w.EnergyDrain).Returns(35);
            Assert.False(Validate(ship, "KineticDifference"));
            
            weapon.Setup(w => w.EnergyDrain).Returns(34);
            Assert.True(Validate(ship, "KineticDifference"));
        }

        [Fact]
        public void CheckNullifierNotOnlyWeaponOnWing()
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            
            var weapon1 = WeaponFactory.CreateWeapon();
            weapon1.Setup(w => w.Id).Returns(14);

            wing.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object}).ToList());
            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());
            
            Assert.False(Validate(ship, "LoneNullifier"));

            var weapon2 = WeaponFactory.CreateWeapon();

            wing.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object, weapon2.Object}).ToList());

            Assert.True(Validate(ship, "LoneNullifier"));
        }

        [Theory]
        [InlineData(Licence.A, -1)]
        [InlineData(Licence.A, 0)]
        [InlineData(Licence.A, 1)]
        [InlineData(Licence.B, 0)]
        [InlineData(Licence.C, 0)]
        [InlineData(Licence.Z, -1)]
        [InlineData(Licence.Z, 0)]
        [InlineData(Licence.Z, 1)]
        public void CheckMaxLicenseWeight(Licence licence, int weightModifier)
        {
            var ship = ShipFactory.CreateShip();

            var calculations = new Mock<Calculations>();
            calculations.Setup(c => c.GetShipWeight(ship.Object)).Returns((int) licence + weightModifier);

            var valid = Validate(ship, "ToHeavyForLicense", calculations, licence: licence);
            
            if (weightModifier <= 0 || licence == Licence.Z)
                Assert.True(valid);
            else
                Assert.False(valid);

        }
    }
}
