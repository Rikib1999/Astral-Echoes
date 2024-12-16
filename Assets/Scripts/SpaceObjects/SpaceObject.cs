using Assets.Scripts.Enums;
using Assets.Scripts.Structs;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Netcode;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.SpaceObjects
{
    // Generic abstract class for space objects, supporting randomization and customization
    // T represents the enum type for the object's subtypes
    public abstract class SpaceObject<T> : MonoBehaviour where T : Enum
    {
        // Abstract properties to define minimum and maximum size for the space object
        protected abstract float MinSize { get; }
        protected abstract float MaxSize { get; }

        public eSpaceObjectType Type { get; set; } // Type of the space object (e.g., planet, star)
        public T SubType { get; set; } // Subtype of the space object, defined by the generic enum T
        public string Name { get; set; } // Name of the space object
        public float Size => transform.localScale.x; // Size of the space object, derived from its scale
        public bool IsLandable { get; protected set; } // Indicates if the object can be landed on
        protected Vector2 Coordinates { get; set; } // Coordinates of the space object in the game world

        // Method to randomize the space object's properties
        public virtual void Randomize()
        {
            Coordinates = transform.position; // Set the coordinates to the object's current position
            SetName(); // Generate a random name
            SetIsLandable(); // Determine if the object is landable
            SetSize(); // Randomize the object's size
            SetSubType(); // Assign a random subtype
            SetSprite(); // Set the object's sprite based on its type and subtype
            SetTooltip(); // Configure the tooltip for the object
        }

        // Generates a random name for the space object
        private void SetName()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[9]; // Generate a name with 9 random characters

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[Random.Range(0, chars.Length)];
            }

            Name = new string(stringChars); // Convert the character array into a string
        }

        // Determines if the object is landable based on its type
        protected void SetIsLandable()
        {
            IsLandable = Type == eSpaceObjectType.Planet; // Only planets are landable
        }

        // Sets the size of the space object within its allowed range
        private void SetSize()
        {
            float scale = Random.Range(MinSize, MaxSize); // Randomize size between MinSize and MaxSize
            transform.localScale = new Vector2(scale, scale); // Apply the size to the object's scale
        }

        // Randomly selects a subtype for the space object from the enum T
        private void SetSubType()
        {
            var subTypes = Enum.GetValues(typeof(T)); // Get all possible subtypes
            int maxIndex = subTypes.Length - 1; // Determine the upper bound for random selection
            int index = Random.Range(0, maxIndex); // Pick a random index
            SubType = (T)subTypes.GetValue(index); // Assign the corresponding subtype
        }

        // Assigns a random sprite to the space object based on its type and subtype
        public void SetSprite()
        {
            int maxIndex = SpaceObjectSpriteManager.Instance.storage[Type][SubType].Length - 1; // Find the number of available sprites
            int index = Random.Range(0, maxIndex); // Pick a random sprite index
            GetComponent<SpriteRenderer>().sprite = SpaceObjectSpriteManager.Instance.storage[Type][SubType][index]; // Set the sprite
        }

        // Configures the tooltip for the space object, optionally scaling its size display
        public void SetTooltip(float scaleDownConst = 1)
        {
            GetComponent<TooltipSetter>().tooltipData = new TooltipData(
                Name, // Object's name
                Type, // Object's type
                SubType, // Object's subtype
                Coordinates.x, // X-coordinate
                Coordinates.y, // Y-coordinate
                Size / scaleDownConst, // Scaled size
                0, // Additional data (e.g., distance, initially set to 0)
                IsLandable // Whether the object is landable
            );
        }

        // Updates the tooltip with travel information, including distance to the object
        public void SetTooltipDistance(bool canTravel, float distance)
        {
            var t = GetComponent<TooltipSetter>();
            t.tooltipData.canTravel = canTravel; // Indicates if travel to the object is possible
            t.tooltipData.distance = distance; // Sets the distance to the object
        }

        // Allows manual setting of the object's size
        public void SetSize(float size)
        {
            transform.localScale = new Vector3(size, size, 1); // Update the object's scale
        }

        // Allows manual setting of the object's subtype
        public void SetSubType(Enum subType)
        {
            SubType = (T)subType; // Cast the provided enum to type T
        }

        // Allows manual setting of the object's name
        public void SetName(string name)
        {
            Name = name;
        }

        // Allows manual setting of the object's coordinates
        public void SetCoordinates(Vector2 coords)
        {
            Coordinates = coords;
        }
    }
}