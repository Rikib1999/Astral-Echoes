using Assets.Scripts.Structs;
using System;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.SpaceObjects
{
    // Abstract class representing a satellite object with orbiting behavior.
    public abstract class SatelliteObject<T> : SpaceObject<T> where T : Enum
    {
        // The position of the central object this satellite orbits around.
        public Vector3 CentreObjectPosition { get; set; }

        // The radius of the orbit around the central object.
        public float OrbitRadius { get; set; }

        // Reference to the LineRenderer component used to draw the orbit.
        [SerializeField] private LineRenderer lineRenderer;

        // Randomizes properties of the satellite object, including rotation and tooltip setup.
        public override void Randomize()
        {
            base.Randomize();

            // Set the satellite's initial rotation.
            SetRotation();

            // Set up tooltip information for the satellite.
            SetTooltip();
        }

        // Configures the LineRenderer with default visual settings.
        private void SetLineRenderer()
        {
            lineRenderer.startColor = new Color(1, 1, 1, 0.25f); // Semi-transparent white.
            lineRenderer.endColor = new Color(1, 1, 1, 0.25f);
            lineRenderer.startWidth = 0.2f; // Constant line width.
            lineRenderer.endWidth = 0.2f;
        }

        // Sets the satellite's rotation based on its position relative to the central object.
        private void SetRotation()
        {
            // Calculate a normalized vector pointing towards the central object.
            Vector3 v = (CentreObjectPosition - transform.position).normalized;

            // Determine the angle in degrees between the vector and the x-axis.
            float r = (float)(Math.Atan2(v.y, v.x) * 180 / Math.PI) - 41;

            // Apply the calculated rotation to the satellite.
            transform.Rotate(0, 0, r);
        }

        // Sets up tooltip information for the satellite.
        public new void SetTooltip(float scaleDownConst = 1)
        {
            // Assign tooltip data using satellite properties and optional scaling factors.
            GetComponent<TooltipSetter>().tooltipData = new TooltipData(
                Name,
                Type,
                SubType,
                Coordinates.x,
                Coordinates.y,
                Size / scaleDownConst,
                OrbitRadius / scaleDownConst,
                IsLandable
            );
        }

        // Configures the satellite's orbit with a given center position and radius.
        public void SetOrbit(Vector2 centreObjectPosition, float orbitRadius)
        {
            // Initialize the LineRenderer settings.
            SetLineRenderer();

            // Set the central object's position and orbit radius.
            CentreObjectPosition = new Vector3(centreObjectPosition.x, centreObjectPosition.y, 0);
            OrbitRadius = orbitRadius;

            // Draw the orbit with a specified number of steps.
            DrawOrbit(100);
        }

        // Draws the satellite's orbit as a circle using the LineRenderer.
        private void DrawOrbit(int steps)
        {
            // Set the number of points in the orbit line.
            lineRenderer.positionCount = steps;

            // Calculate each point along the orbit.
            for (int i = 0; i < steps; i++)
            {
                // Determine progress around the circle (0 to 1).
                float progress = (float)i / (steps - 1);

                // Convert progress to radians for circular motion.
                float radian = progress * 2 * Mathf.PI;

                // Calculate x and y components of the orbit point.
                float xScaled = Mathf.Cos(radian);
                float yScaled = Mathf.Sin(radian);

                float x = xScaled * OrbitRadius;
                float y = yScaled * OrbitRadius;

                // Create the position vector for the orbit point.
                Vector3 position = new Vector3(x, y, 0);

                // Set the point's position in the LineRenderer, adjusted for the satellite's position.
                lineRenderer.SetPosition(i, transform.position + position + (CentreObjectPosition - transform.position));
            }
        }
    }
}