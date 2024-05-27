using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class BlackHole : SpaceObject<eBlackHoleType>
    {
        protected override float MinSize { get; } = 0.5f;
        protected override float MaxSize { get; } = 6f;

        private new void Start()
        {
            Type = eSpaceObjectType.BlackHole;
            base.Start();
        }
    }
}