using UnityEngine;

namespace Assets.Scripts
{
    public struct ChunkDimensions
    {
        public Vector2 centre;
        public float top;
        public float bottom;
        public float left;
        public float right;

        public ChunkDimensions(float size, float radius, Vector2Int coords)
        {
            centre = new((size * coords.x) + radius, (size * coords.y) + radius);
            top = centre.y + radius;
            bottom = centre.y - radius;
            right = centre.x + radius;
            left = centre.x - radius;
        }
    }
}