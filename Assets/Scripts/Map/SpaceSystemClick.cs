using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using UnityEngine;
using Unity.Netcode;

public class SpaceSystemClick : NetworkBehaviour
{
    //public SystemDataBag systemDataBag;

    private void OnMouseUpAsButton()
    {
        var bag = this.gameObject.GetComponent<SystemDataBag>();
        //Debug.Log(JsonUtility.ToJson(bag, true));
        SystemMapManager.Instance.EnterSystem(bag);
    }
}