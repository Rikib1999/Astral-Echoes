using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class BlackHole : SpaceObject<eBlackHoleType>
    {
        protected override float MinSize { get; } = 0.5f;
        protected override float MaxSize { get; } = 6f;

        public new void Randomize()
        {
            Type = eSpaceObjectType.BlackHole;
            base.Randomize();
        }
    }
}