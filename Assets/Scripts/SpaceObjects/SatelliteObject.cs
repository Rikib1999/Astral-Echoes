using Assets.Scripts.Structs;
using System;
using UnityEngine;

namespace Assets.Scripts.SpaceObjects
{
    public abstract class SatelliteObject<T> : SpaceObject<T> where T : Enum
    {
        public Vector3 CentreObjectPosition { get; set; }
        public float OrbitRadius { get; set; }

        [SerializeField] private LineRenderer lineRenderer;

        protected virtual new void Awake()
        {
            base.Awake();

            SetRotation();
            SetTooltip();
        }

        private void SetLineRenderer()
        {
            lineRenderer.startColor = new Color(1, 1, 1, 0.25f);
            lineRenderer.endColor = new Color(1, 1, 1, 0.25f);
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
        }

        private void SetRotation()
        {
            Vector3 v = (CentreObjectPosition - transform.position).normalized;

            float r = (float)(Math.Atan2(v.y, v.x) * 180 / Math.PI) - 41;

            transform.Rotate(0, 0, r);
        }

        public new void SetTooltip(float scaleDownConst = 1)
        {
            GetComponent<TooltipSetter>().tooltipData = new TooltipData(Name, Type, SubType, Coordinates.x, Coordinates.y, Size / scaleDownConst, OrbitRadius / scaleDownConst, IsLandable);
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