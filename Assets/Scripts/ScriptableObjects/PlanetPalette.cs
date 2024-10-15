using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PlanetPalette", menuName = "ScriptableObjects/PlanetPalette", order = 2)]
public class PlanetPalette : ScriptableObject
{
    public TileList[] tileList;
    public float[] levels;
    public bool[] isWalkable;
}

[Serializable]
public class TileList
{
    public Tile[] tiles;
}