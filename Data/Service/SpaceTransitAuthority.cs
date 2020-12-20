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
            Hulls = new List<Hull> {
                new Hull {
                    Id = 1,
                    Name = "Zenith",
                    DefaultMaximumTakeOffMass = TakeOffMassEnum.LightFighter
                },
                new Hull {
                    Id = 2,
                    Name = "Neptunus",
                    DefaultMaximumTakeOffMass = TakeOffMassEnum.Tank
                },
                new Hull {
                    Id = 3,
                    Name = "Catalyst",
                    DefaultMaximumTakeOffMass = TakeOffMassEnum.MediumFighter
                },
                new Hull {
                    Id = 4,
                    Name = "RaceWing",
                    DefaultMaximumTakeOffMass = TakeOffMassEnum.Interceptor
                }};
            Weapons = new List<Weapon> { 
                new Weapon {
                    Id = 1,
                    Name = "Fury Cannon",
                    Damage = 34,
                    EnergyDrain = 52
                },
                new Weapon {
                    Id = 2,
                    Name= "Crusher",
                    Damage = 21,
                    EnergyDrain = 56
                },
                new Weapon {
                    Id = 3,
                    Name= "Flamethrower",
                    Damage = 18,
                    DamageType = DamageTypeEnum.Heat,
                    EnergyDrain = 74
                },
                new Weapon
                {
                    Id = 4,
                    Name = "Freeze Ray",
                    Damage = 24,
                    DamageType = DamageTypeEnum.Cold,
                    EnergyDrain = 52
                },
                new Weapon {
                    Id= 5,
                    Name= "Shockwave",
                    Damage= 18,
                    DamageType = DamageTypeEnum.Kinetic,
                    EnergyDrain= 47
                },
                new Weapon {
                    Id= 6,
                    Name= "Gauss Gun",
                    Damage = 31,
                    DamageType = DamageTypeEnum.Kinetic,
                    EnergyDrain=52
                },
                new Weapon {
                    Id=7,
                    Name="Hailstorm",
                    Damage=21,
                    DamageType= DamageTypeEnum.Cold,
                    EnergyDrain= 56
                },
                new Weapon {
                    Id=8,
                    Name= "Ice Barrage",
                    Damage= 19,
                    DamageType= DamageTypeEnum.Cold,
                    EnergyDrain=41
                },
                new Weapon {
                    Id=9,
                    Name= "Imploder",
                    Damage = 27,
                    DamageType= DamageTypeEnum.Gravity,
                    EnergyDrain=43
                },
                new Weapon {
                    Id= 10,
                    Name= "Levitator",
                    Damage=21,
                    EnergyDrain=56,
                    DamageType = DamageTypeEnum.Statis
                },
                new Weapon {
                    Id= 11,
                    Name= "Shredder",
                    Damage= 10,
                    EnergyDrain= 13,
                    DamageType = DamageTypeEnum.Kinetic
                },
                new Weapon {
                    Id= 12,
                    Name= "Tidal Wave",
                    Damage= 18,
                    DamageType= DamageTypeEnum.Statis,
                    EnergyDrain=74
                },
                new Weapon{
                    Id= 13,
                    Name="Volcano",
                    Damage= 10,
                    DamageType = DamageTypeEnum.Heat,
                    EnergyDrain = 10,
                },
                new Weapon {
                    Id = 14,
                    Name= "Nullifier",
                    Damage= 23,
                    DamageType= DamageTypeEnum.Gravity,
                    EnergyDrain = 43
                }};
            Wings = new List<Wing> {
                new Wing {Id = 1, Name = "Blade",  Energy = 0, WeaponSlots = 2, Weight = 275 },
                new Wing {Id = 2, Name = "Horizon", Energy = 0, WeaponSlots = 1, Weight = 150 },
                new Wing {Id = 3, Name = "D-Fence",  Energy = 0, WeaponSlots = 3, Weight = 300 },
                new Wing {Id = 4, Name = "O-Fence", Energy = 15, WeaponSlots = 2, Weight = 250 },
                new Wing {Id = 5, Name = "Racer",  Energy = 5, WeaponSlots = 1, Weight = 175 },
            };
            Engines = new List<Engine> {
                new Engine {Id = 1, Name = "Galaxy Class", Energy = 150 },
                new Engine {Id = 2, Name = "Intrepid Class", Energy = 350 },
                new Engine {Id = 3, Name = "Constellation Class", Energy = 200 }
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
