using Assets.Scripts.Enums;
using Assets.Scripts.Structs;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Netcode;

namespace Assets.Scripts.SpaceObjects
{
    public abstract class SpaceObject<T> : NetworkBehaviour where T : Enum
    {
        protected abstract float MinSize { get; }
        protected abstract float MaxSize { get; }

        public eSpaceObjectType Type { get; set; }
        public T SubType { get; set; }
        public string Name { get; set; }
        public float Size => transform.localScale.x;
        public bool IsLandable { get; protected set; }
        protected Vector2 Coordinates { get; set; }

        public override void OnNetworkSpawn()
        {
            Debug.Log("Spawn "+Name);
        }

        public virtual void Randomize()
        {
            Coordinates = transform.position;
            SetName();
            SetIsLandable();
            SetSize();
            SetSubType();
            SetSprite();
            SetTooltip();
        }

        private void SetName()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[9];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[Random.Range(0, chars.Length)];
            }

            Name = new string(stringChars);
        }

        protected void SetIsLandable()
        {
            IsLandable = Type == eSpaceObjectType.Planet;
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

        public void SetSprite()
        {
            int maxIndex = SpaceObjectSpriteManager.Instance.storage[Type][SubType].Length - 1;
            int index = Random.Range(0, maxIndex);
            GetComponent<SpriteRenderer>().sprite = SpaceObjectSpriteManager.Instance.storage[Type][SubType][index];
        }

        public void SetTooltip(float scaleDownConst = 1)
        {
            GetComponent<TooltipSetter>().tooltipData = new TooltipData(Name, Type, SubType, Coordinates.x, Coordinates.y, Size / scaleDownConst, 0, IsLandable);
        }

        public void SetSize(float size)
        {
            transform.localScale = new Vector3(size, size, 1);
        }

        public void SetSubType(Enum subType)
        {
            SubType = (T)subType;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetCoordinates(Vector2 coords)
        {
            Coordinates = coords;
        }
    }
}