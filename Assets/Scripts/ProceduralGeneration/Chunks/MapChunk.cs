using Assets.Scripts.SpaceObjects;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Assets.Scripts
{
    public class MapChunk : Chunk
    {
        public List<GameObject> SpaceObjects { get; set; }
    }
}