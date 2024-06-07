using UnityEngine;

namespace Assets.Scripts
{
    public readonly struct ChunkDimensions
    {
        public readonly Vector2 centre;
        public readonly float top;
        public readonly float bottom;
        public readonly float left;
        public readonly float right;

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