using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.SpaceObjects
{
    public abstract class SpaceObject : MonoBehaviour
    {
        protected abstract float MinSize { get; }
        protected abstract float MaxSize { get; }

        public eSpaceObjectType Type { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public float Size => transform.localScale.x;

        protected void Start()
        {
            SetSize();
            SetSprite();
        }

        private void SetSize()
        {
            float scale = Random.Range(MinSize, MaxSize);
            transform.localScale = new Vector2(scale, scale);
        }

        protected abstract void SetSprite();
    }
}