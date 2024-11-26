using UnityEngine;

[CreateAssetMenu(fileName = "PlanetResourcesList", menuName = "ScriptableObjects/PlanetResourcesList", order = 4)]
public class PlanetResourcesList : ScriptableObject
{
    public PlanetResources[] trees;
    public PlanetResources[] bushes;
    public PlanetResources[] stones;
    public PlanetResources[] crystals;
}