using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Web.Utils;
using Xunit;
using Xunit.Abstractions;
using xUnitTest.Factories;

namespace xUnitTest
{
    public class CalculationsTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Calculations _calculations;

        public CalculationsTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _calculations = new Calculations();
        }
        
        [Fact]
        public void CheckNormalShipWeight()
        {
            var ship = ShipFactory.CreateShip();
            var engine = EngineFactory.CreateEngine();
            var wing1 = WingFactory.CreateWing();
            var wing2 = WingFactory.CreateWing();

            wing1.Setup(w => w.Weight).Returns(5);
            wing2.Setup(w => w.Weight).Returns(10);

            var weapon1 = WeaponFactory.CreateWeapon();
            weapon1.Setup(w => w.Weight).Returns(15);
            var weapon2 = WeaponFactory.CreateWeapon();
            weapon2.Setup(w => w.Weight).Returns(20);

            wing1.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object}).ToList());
            wing2.Setup(w => w.Hardpoint).Returns((new[] {weapon2.Object}).ToList());
            
            engine.Setup(e => e.Weight).Returns(25);
            
            ship.Setup(s => s.Engine).Returns(engine.Object);
            ship.Setup(s => s.Wings).Returns((new[] {wing1.Object, wing2.Object}).ToList());

            Assert.Equal(75, _calculations.GetShipWeight(ship.Object));
        }
        
        [Fact]
        public void CheckStatisShipWeight()
        {
            var ship = ShipFactory.CreateShip();
            var engine = EngineFactory.CreateEngine();
            var wing1 = WingFactory.CreateWing();
            var wing2 = WingFactory.CreateWing();

            wing1.Setup(w => w.Weight).Returns(5);
            wing2.Setup(w => w.Weight).Returns(10);

            var weapon1 = WeaponFactory.CreateWeapon();
            weapon1.Setup(w => w.Weight).Returns(15);
            weapon1.Setup(w => w.DamageType).Returns(DamageTypeEnum.Statis);
            var weapon2 = WeaponFactory.CreateWeapon();
            weapon2.Setup(w => w.Weight).Returns(20);
            weapon2.Setup(w => w.DamageType).Returns(DamageTypeEnum.Statis);

            wing1.Setup(w => w.Hardpoint).Returns((new[] {weapon1.Object}).ToList());
            wing2.Setup(w => w.Hardpoint).Returns((new[] {weapon2.Object}).ToList());
            
            engine.Setup(e => e.Weight).Returns(25);
            
            ship.Setup(s => s.Engine).Returns(engine.Object);
            ship.Setup(s => s.Wings).Returns((new[] {wing1.Object, wing2.Object}).ToList());

            Assert.Equal(75*0.85, _calculations.GetShipWeight(ship.Object));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void CheckEnergyConsumptionSameType(int weaponAmount)
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
           
            var weapons = new List<Weapon>();

            for (var i = 0; i < weaponAmount; i++)
            {
                var weapon = WeaponFactory.CreateWeapon();
                weapon.Setup(w => w.EnergyDrain).Returns(5);
                weapon.Setup(w => w.DamageType).Returns(DamageTypeEnum.Statis);
                weapons.Add(weapon.Object);
            }
            
            wing.Setup(w => w.Hardpoint).Returns(weapons);
            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());

            Assert.Equal(weaponAmount* 5 * (weaponAmount >= 3 ? 0.8 : 1), _calculations.GetEnergyConsumption(ship.Object));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void CheckEnergyConsumptionNotSameType(int weaponAmount)
        {
            var ship = ShipFactory.CreateShip();
            var wing = WingFactory.CreateWing();
            
            var weapons = new List<Weapon>();

            for (var i = 0; i < weaponAmount; i++)
            {
                var weapon = WeaponFactory.CreateWeapon();
                weapon.Setup(w => w.EnergyDrain).Returns(5);
                weapon.Setup(w => w.DamageType).Returns((DamageTypeEnum) i);
                _testOutputHelper.WriteLine(((DamageTypeEnum) i).ToString());
                weapons.Add(weapon.Object);
            }
            
            wing.Setup(w => w.Hardpoint).Returns(weapons);
            ship.Setup(s => s.Wings).Returns((new[] {wing.Object}).ToList());

            Assert.Equal(weaponAmount* 5, _calculations.GetEnergyConsumption(ship.Object));
        }
    }
}