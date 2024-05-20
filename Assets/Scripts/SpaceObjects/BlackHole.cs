using UnityEngine;

namespace Assets.Scripts.SpaceObjects
{
    public class BlackHole : SpaceObject
    {
        protected override float MinSize { get; } = 0.5f;
        protected override float MaxSize { get; } = 6f;

        private new void Start()
        {
            base.Start();
        }

        protected override void SetSprite()
        {
            //int maxIndex = spaceObjectsImageCollection.blackHoles.Length - 1;
            //int index = Random.Range(0, maxIndex);
            //GetComponent<SpriteRenderer>().sprite = spaceObjectsImageCollection.blackHoles[index];
        }
    }
}