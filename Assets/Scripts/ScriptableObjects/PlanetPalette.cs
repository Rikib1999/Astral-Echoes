using UnityEngine;

[CreateAssetMenu(fileName = "PlanetPalette", menuName = "ScriptableObjects/PlanetPalette", order = 2)]
public class PlanetPalette : ScriptableObject
{
    public RuleTile[] tiles;
    public float[] levels;
    public bool[] isWalkable;
}