using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PlanetPalette", menuName = "ScriptableObjects/PlanetPalette", order = 2)]
public class PlanetPalette : ScriptableObject
{
    public Tile[] tiles;
    public float[] levels;
    public bool[] isWalkable;
}