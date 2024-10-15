using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class Planet : SatelliteObject<ePlanetType>
    {
        protected override float MinSize { get; } = 0.3f;
        protected override float MaxSize { get; } = 2f;

        protected override void Awake()
        {
            Type = eSpaceObjectType.Planet;
            base.Awake();

            SetIsLandable();
        }

        public new void SetIsLandable()
        {
            IsLandable = SubType != ePlanetType.Ocean;
        }
    }
}