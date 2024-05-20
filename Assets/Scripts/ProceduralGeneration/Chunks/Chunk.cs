using UnityEngine;

namespace Assets.Scripts
{
    public class Chunk
    {
        public Vector2Int Coords { get; set; }
        public bool Visible { get; set; }
        public double CoordsDistance { get; set; }
    }
}