using Assets.Scripts.Enums;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.SpaceObjects
{
    public abstract class SpaceObject<T> : MonoBehaviour where T : Enum
    {
        protected abstract float MinSize { get; }
        protected abstract float MaxSize { get; }

        public eSpaceObjectType Type { get; set; }
        public T SubType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Size => transform.localScale.x;

        protected void Start()
        {
            SetSize();
            SetSubType();
            SetSprite();
        }

        private void SetSize()
        {
            float scale = Random.Range(MinSize, MaxSize);
            transform.localScale = new Vector2(scale, scale);
        }

        private void SetSubType()
        {
            var subTypes = Enum.GetValues(typeof(T));
            int maxIndex = subTypes.Length - 1;
            int index = Random.Range(0, maxIndex);
            SubType = (T)subTypes.GetValue(index);
        }

        private void SetSprite()
        {
            int maxIndex = SpaceObjectSpriteManager.Instance.storage[Type][SubType].Length - 1;
            int index = Random.Range(0, maxIndex);
            GetComponent<SpriteRenderer>().sprite = SpaceObjectSpriteManager.Instance.storage[Type][SubType][index];
        }
    }
}