using Assets.Scripts.Structs;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI coordinatesText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI orbitDistanceLabel;
    public TextMeshProUGUI orbitDistanceText;
    public TextMeshProUGUI isLandableText;
    public TextMeshProUGUI distanceText;

    private void Start()
    {
        Instance = this;
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ShowToolTip(TooltipData tooltipData)
    {
        gameObject.SetActive(true);

        nameText.text = tooltipData.name;
        typeText.text = tooltipData.subType.ToString() + " " + Regex.Replace(tooltipData.type.ToString(), @"([a-z])([A-Z])", "$1 $2");
        coordinatesText.text = tooltipData.posX.ToString("0.000") + "\n" + tooltipData.posY.ToString("0.000");
        sizeText.text = tooltipData.size.ToString("0.00");

        if (tooltipData.orbitDistance > 0.01f)
        {
            orbitDistanceLabel.text = "  Orbit distance:";
            orbitDistanceText.text = tooltipData.orbitDistance.ToString("0.00");
        }
        else
        {
            orbitDistanceLabel.text = string.Empty;
            orbitDistanceText.text = string.Empty;
        }

        if (tooltipData.isLandable)
        {
            isLandableText.text = "Landable";
            isLandableText.color = Color.green;
        }
        else
        {
            isLandableText.text = "Not landable";
            isLandableText.color = Color.gray;
        }

        if (tooltipData.distance == 0)
        {
            distanceText.text = "Current system";
        }
        else if (tooltipData.canTravel)
        {
            distanceText.text = "Fuel needed: " + tooltipData.distance;
        }
        else
        {
            distanceText.text = "Not enough fuel\n(" + tooltipData.distance + ")";
        }
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);

        nameText.text = string.Empty;
        typeText.text = string.Empty;
        coordinatesText.text = string.Empty;
        sizeText.text = string.Empty;
        orbitDistanceText.text = string.Empty;
        isLandableText.text = string.Empty;
        distanceText.text = string.Empty;
    }
}