using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SpaceSystem
{
    public class SystemDataBag
    {
        public SpaceObjectDataBag CentralObject { get; set; }
        public List<SpaceObjectDataBag> SatelliteObjects { get; set; } = new();
    }

    public class SpaceObjectDataBag
    {
        public string Name { get; set; }
        public eSpaceObjectType Type { get; set; }
        public Enum SubType { get; set; }
        public float Size { get; set; }
        public Vector2 Coordinates { get; set; }
        public Vector2 RelativePosition { get; set; }
        public float OrbitRadius { get; set; }
    }
}