using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class GasGiant : SatelliteObject<eGasGiantType>
    {
        protected override float MinSize { get; } = 1.5f;
        protected override float MaxSize { get; } = 3.5f;

        private new void Start()
        {
            Type = eSpaceObjectType.GasGiant;
            base.Start();
        }
    }
}