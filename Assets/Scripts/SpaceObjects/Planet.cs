using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.SpaceObjects
{
    public class Planet : SatelliteObject
    {
        protected override float MinSize { get; } = 0.3f;
        protected override float MaxSize { get; } = 2f;

        public ePlanetType PlanetType { get; set; }

        private new void Start()
        {
            base.Start();

            //GetComponent<SpriteRenderer>().sprite = spaceObjectsImageCollection.airlessPlanets[0];
        }

        protected override void SetSprite()
        {
            throw new System.NotImplementedException();
        }
    }
}