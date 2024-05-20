using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class GasGiant : SatelliteObject
    {
        protected override float MinSize { get; } = 1.5f;
        protected override float MaxSize { get; } = 3.5f;

        public eGasGiantType GasGiantType { get; set; }

        private new void Start()
        {
            base.Start();
        }

        protected override void SetSprite()
        {
            throw new System.NotImplementedException();
        }
    }
}