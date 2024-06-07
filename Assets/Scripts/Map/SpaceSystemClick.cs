using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using UnityEngine;

public class SpaceSystemClick : MonoBehaviour
{
    public SystemDataBag systemDataBag;

    private void OnMouseUpAsButton()
    {
        SystemMapManager.Instance.EnterSystem(systemDataBag);
    }
}