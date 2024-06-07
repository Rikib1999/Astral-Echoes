using Assets.Scripts.Structs;
using UnityEngine;

public class TooltipSetter : MonoBehaviour
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