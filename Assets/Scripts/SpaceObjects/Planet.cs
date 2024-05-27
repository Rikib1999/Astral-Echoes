using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class Planet : SatelliteObject<ePlanetType>
    {
        protected override float MinSize { get; } = 0.3f;
        protected override float MaxSize { get; } = 2f;

        private new void Start()
        {
            Type = eSpaceObjectType.Planet;
            base.Start();
        }
    }
}