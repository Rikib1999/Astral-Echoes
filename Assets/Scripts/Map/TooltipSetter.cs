using Assets.Scripts.Structs;
using UnityEngine;
using Unity.Netcode;

public class TooltipSetter : NetworkBehaviour
{
    public TooltipData tooltipData;

    private void OnMouseEnter()
    {
        TooltipManager.Instance.ShowToolTip(tooltipData);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideToolTip();
    }
}