using Assets.Scripts.Enums;
using System;

namespace Assets.Scripts.Structs
{
    public readonly struct TooltipData
    {
        public readonly string name;
        public readonly eSpaceObjectType type;
        public readonly Enum subType;
        public readonly float posX;
        public readonly float posY;
        public readonly float size;
        public readonly float orbitDistance;
        public readonly bool isLandable;

        public TooltipData(string name, eSpaceObjectType type, Enum subType, float posX, float posY, float size, float orbitDistance, bool isLandable)
        {
            this.name = name;
            this.type = type;
            this.subType = subType;
            this.posX = posX;
            this.posY = posY;
            this.size = size;
            this.orbitDistance = orbitDistance;
            this.isLandable = isLandable;
        }
    }
}