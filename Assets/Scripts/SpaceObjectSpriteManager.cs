﻿using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpaceObjectSpriteManager : Singleton<SpaceObjectSpriteManager>
    {
        [SerializeField] protected SpaceObjectsImageCollection spaceObjectsImageCollection;

        public Dictionary<eSpaceObjectType, Dictionary<Enum, Sprite[]>> storage;

        protected override void Awake()
        {
            FillStorage();
            base.Awake();
        }

        private void FillStorage()
        {
            storage = new()
            {
                {
                    eSpaceObjectType.BlackHole,
                    new Dictionary<Enum, Sprite[]>()
                    {
                        {
                            eBlackHoleType.Blue,
                            spaceObjectsImageCollection.blueBlackHoles
                        }
                    }
                },
                {
                    eSpaceObjectType.Star,
                    new Dictionary<Enum, Sprite[]>()
                    {
                        {
                            eStarType.Blue,
                            spaceObjectsImageCollection.blueStars
                        },
                        {
                            eStarType.Red,
                            spaceObjectsImageCollection.redStars
                        },
                        {
                            eStarType.Yellow,
                            spaceObjectsImageCollection.yellowStars
                        }
                    }
                },
                {
                    eSpaceObjectType.Planet,
                    new Dictionary<Enum, Sprite[]>()
                    {
                        {
                            ePlanetType.Airless,
                            spaceObjectsImageCollection.airlessPlanets
                        },
                        {
                            ePlanetType.Aquamarine,
                            spaceObjectsImageCollection.aquamarinePlanets
                        },
                        {
                            ePlanetType.Arid,
                            spaceObjectsImageCollection.aridPlanets
                        },
                        {
                            ePlanetType.Barren,
                            spaceObjectsImageCollection.barrenPlanets
                        },
                        {
                            ePlanetType.Cloudy,
                            spaceObjectsImageCollection.cloudyPlanets
                        },
                        {
                            ePlanetType.Cratered,
                            spaceObjectsImageCollection.crateredPlanets
                        },
                        {
                            ePlanetType.Dry,
                            spaceObjectsImageCollection.dryPlanets
                        },
                        {
                            ePlanetType.Frozen,
                            spaceObjectsImageCollection.frozenPlanets
                        },
                        {
                            ePlanetType.Glacial,
                            spaceObjectsImageCollection.glacialPlanets
                        },
                        {
                            ePlanetType.Icy,
                            spaceObjectsImageCollection.icyPlanets
                        },
                        {
                            ePlanetType.Lunar,
                            spaceObjectsImageCollection.lunarPlanets
                        },
                        {
                            ePlanetType.Lush,
                            spaceObjectsImageCollection.lushPlanets
                        },
                        {
                            ePlanetType.Magma,
                            spaceObjectsImageCollection.magmaPlanets
                        },
                        {
                            ePlanetType.Muddy,
                            spaceObjectsImageCollection.muddyPlanets
                        },
                        {
                            ePlanetType.Oasis,
                            spaceObjectsImageCollection.oasisPlanets
                        },
                        {
                            ePlanetType.Ocean,
                            spaceObjectsImageCollection.oceanPlanets
                        },
                        {
                            ePlanetType.Rocky,
                            spaceObjectsImageCollection.rockyPlanets
                        },
                        {
                            ePlanetType.Snowy,
                            spaceObjectsImageCollection.snowyPlanets
                        },
                        {
                            ePlanetType.Terrestrial,
                            spaceObjectsImageCollection.terrestrialPlanets
                        },
                        {
                            ePlanetType.Tropical,
                            spaceObjectsImageCollection.tropicalPlanets
                        }
                    }
                },
                {
                    eSpaceObjectType.GasGiant,
                    new Dictionary<Enum, Sprite[]>()
                    {
                        {
                            eGasGiantType.Blue,
                            spaceObjectsImageCollection.blueGiants
                        },
                        {
                            eGasGiantType.Green,
                            spaceObjectsImageCollection.greenGiants
                        },
                        {
                            eGasGiantType.Orange,
                            spaceObjectsImageCollection.orangeGiants
                        },
                        {
                            eGasGiantType.Red,
                            spaceObjectsImageCollection.redGiants
                        },
                        {
                            eGasGiantType.Yellow,
                            spaceObjectsImageCollection.yellowGiants
                        }
                    }
                }
            };
        }
    }
}