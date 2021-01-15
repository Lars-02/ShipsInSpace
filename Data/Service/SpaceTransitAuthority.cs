using System;
using System.Collections.Generic;
using System.Text.Json;
using Data.Model;

namespace Data.Service
{
    public class SpaceTransitAuthority : ISpaceTransitAuthority
    {
        private static IEnumerable<Hull> Hulls;
        private static IEnumerable<Weapon> Weapons;
        private static IEnumerable<Wing> Wings;
        private static IEnumerable<Engine> Engines;
        public SpaceTransitAuthority()
        {
           Hulls = new List<Hull> { new Hull {
           Id = 1,
           Name = "Zenith",
           Agility = 135,
           Speed = 120,
           ColdShielding = 0,
           HeatShielding = 50,
           DefaultMaximumTakeOffMass = TakeOffMassEnum.LightFighter
       },
        new Hull
        {
            Id = 2,
            Name = "Neptunus",
            Agility = 100,
            Speed = 100,
            ColdShielding = 50,
            HeatShielding = 0,
            DefaultMaximumTakeOffMass = TakeOffMassEnum.Tank
        },
        new Hull
        {
            Id = 3,
            Name = "Catalyst",
            Agility = 115,
            Speed = 200,
            ColdShielding = 0,
            HeatShielding = 50,
            DefaultMaximumTakeOffMass = TakeOffMassEnum.MediumFighter
        },
        new Hull
        {
            Id = 4,
            Name = "RaceWing",
            Agility = 205,
            Speed = 150,
            ColdShielding = 25,
            HeatShielding = 25,
            DefaultMaximumTakeOffMass = TakeOffMassEnum.Interceptor
        }};
           Weapons = new List<Weapon> { new Weapon {
                    Id = 1,
                    Name = "Fury Cannon",
                    Weight = 76,
					DamageType = DamageTypeEnum.Heat,
                    EnergyDrain = 52
                },
                new Weapon {
                    Id = 2,
                    Name= "Crusher",
                    Weight=89,
					DamageType= DamageTypeEnum.Gravity,
                    EnergyDrain = 56
                },
                new Weapon {
                    Id = 3,
                    Name= "Flamethrower",
                    Weight = 30,
                    DamageType = DamageTypeEnum.Heat,
                    EnergyDrain = 74
                },
                new Weapon
                {
                    Id = 4,
                    Name = "Freeze Ray",
                    Weight = 24,
                    DamageType = DamageTypeEnum.Cold,
                    EnergyDrain = 52
                },
                new Weapon {
                    Id= 5,
                    Name= "Shockwave",
                    Weight= 105,
                    DamageType = DamageTypeEnum.Kinetic,
                    EnergyDrain= 47
                },
                new Weapon {
                    Id= 6,
                    Name= "Gauss Gun",
                    Weight = 110,
                    DamageType = DamageTypeEnum.Kinetic,
                    EnergyDrain=52
                },
                new Weapon {
                    Id=7,
                    Name="Hailstorm",
                    Weight=34,
                    DamageType= DamageTypeEnum.Cold,
                    EnergyDrain= 56
                },
                new Weapon {
                    Id=8,
                    Name= "Ice Barrage",
                    Weight= 35,
                    DamageType= DamageTypeEnum.Cold,
                    EnergyDrain=41
                },
                new Weapon {
                    Id=9,
                    Name= "Imploder",
                    Weight = 270,
                    DamageType= DamageTypeEnum.Gravity,
                    EnergyDrain=43
                },
                new Weapon {
                    Id= 10,
                    Name= "Levitator",
                    Weight=59,
                    EnergyDrain=56,
                    DamageType = DamageTypeEnum.Statis
                },
                new Weapon {
                    Id= 11,
                    Name= "Shredder",
                    Weight= 75,
                    EnergyDrain= 13,
                    DamageType = DamageTypeEnum.Kinetic
                },
                new Weapon {
                    Id= 12,
                    Name= "Tidal Wave",
                    Weight= 18,
                    DamageType= DamageTypeEnum.Statis,
                    EnergyDrain=74
                },
                new Weapon{
                    Id= 13,
                    Name="Volcano",
                    Weight= 80,
                    DamageType = DamageTypeEnum.Heat,
                    EnergyDrain = 10,
                },
                new Weapon {
                    Id = 14,
                    Name= "Nullifier",
                    Weight= 23,
                    DamageType= DamageTypeEnum.Gravity,
                    EnergyDrain = 43
                }};
            Wings = new List<Wing> { 
                new Wing {Id = 1, Name = "Blade", Agility = 0, Speed = 15, Energy = 0, NumberOfHardpoints = 2, Weight = 275 },
                new Wing {Id = 2, Name = "Horizon", Agility = 7, Speed = 8, Energy = 0, NumberOfHardpoints = 1, Weight = 150 },
                new Wing {Id = 3, Name = "D-Fence", Agility = 0, Speed = 0, Energy = 0, NumberOfHardpoints = 3, Weight = 300 },
                new Wing {Id = 4, Name = "O-Fence", Agility = 0, Speed = 0, Energy = 15, NumberOfHardpoints = 2, Weight = 250 },
                new Wing {Id = 5, Name = "Raceing", Agility = 15, Speed = 15, Energy = 0, NumberOfHardpoints = 1, Weight = 175 },
            };
            Engines = new List<Engine> { 
                new Engine {Id = 1, Name = "Galaxy Class", Energy = 150, Weight = 150 },
                new Engine {Id = 2, Name = "Intrepid Class", Energy = 350, Weight = 350 },
                new Engine {Id = 3, Name = "Constellation Class", Energy = 200, Weight= 200 }
            };
        }

        public IEnumerable<Weapon> GetWeapons()
        {
            return Weapons;
        }

        public double CheckActualHullCapacity(Hull hull)
        {
            var stressTest = new Random();
            return (int)hull.DefaultMaximumTakeOffMass - 100 + stressTest.NextDouble() * 200;
        }

        public IEnumerable<Hull> GetHulls()
        {
            return Hulls;
        }

        public IEnumerable<Wing> GetWings()
        {
            return Wings;
        }

        public IEnumerable<Engine> GetEngines()
        {
            return Engines;
        }

        /// <summary>
        /// returns new Registration ID if data is valid, empty string if data is not parseable
        /// </summary>
        /// <param name="JSONString">Should contain a ship configuration</param>
        /// <returns></returns>
        public string RegisterShip(string JSONString)
        {
            try
            {
                var ship = JsonSerializer.Deserialize<Ship>(JSONString);
                return Guid.NewGuid().ToString();
            }
            catch (JsonException)
            {
                return string.Empty;
            }
        }
    }
}
