using UnityEngine;

namespace Assets.Scripts.SpaceObjects
{
    public abstract class SatelliteObject : SpaceObject
    {
        public Vector3 CentreObjectPosition { get; set; }
        public float OrbitRadius { get; set; }

        [SerializeField] private LineRenderer lineRenderer;

        private new void Start()
        {
            base.Start();
        }

        private void SetLineRenderer()
        {
            lineRenderer.startColor = new Color(1, 1, 1, 0.25f);
            lineRenderer.endColor = new Color(1, 1, 1, 0.25f);
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
        }

        public void SetOrbit(Vector2 centreObjectPosition, float orbitRadius)
        {
            SetLineRenderer();

            CentreObjectPosition = new(centreObjectPosition.x, centreObjectPosition.y, 0);
            OrbitRadius = orbitRadius;
            DrawOrbit(100);
        }

        private void DrawOrbit(int steps)
        {
            lineRenderer.positionCount = steps;

            for (int i = 0; i < steps; i++)
            {
                float progress = (float)i / (steps - 1);
                float radian = progress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(radian);
                float yScaled = Mathf.Sin(radian);

                float x = xScaled * OrbitRadius;
                float y = yScaled * OrbitRadius;

                Vector3 position = new(x, y, 0);

                lineRenderer.SetPosition(i, transform.position + position + (CentreObjectPosition - transform.position));
            }
        }
    }
}