using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpaceObjectSpriteManager : Singleton<SpaceObjectSpriteManager>
    {
        [SerializeField] protected SpaceObjectsImageCollection spaceObjectsImageCollection;

        [SerializeField] public Dictionary<eSpaceObjectType, Dictionary<Enum, Sprite[]>> storage;

        private void Start()
        {
            FillStorage();
        }

        private void FillStorage()
        {

        }
    }
}