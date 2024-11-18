using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using UnityEngine;

public class PlanetClick : MonoBehaviour
{
    [SerializeField] public SpaceObjectDataBag spaceObjectDataBag;

    private void OnMouseUpAsButton()
    {
        PlanetMapManager.Instance.LandPlanet(spaceObjectDataBag);
    }
}