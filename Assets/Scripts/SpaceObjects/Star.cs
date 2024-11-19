using Assets.Scripts.Enums;

namespace Assets.Scripts.SpaceObjects
{
    public class Star : SpaceObject<eStarType>
    {
        protected override float MinSize { get; } = 3.5f;
        protected override float MaxSize { get; } = 6f;

        public new void Randomize()
        {
            Type = eSpaceObjectType.Star;
            base.Randomize();
        }
    }
}