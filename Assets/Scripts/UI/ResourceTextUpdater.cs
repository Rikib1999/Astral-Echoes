using TMPro;
using UnityEngine;

public class ResourceTextUpdater : MonoBehaviour
{
    public TMP_Text waterText;
    public TMP_Text foodText;
    public TMP_Text energyText;
    public TMP_Text fuelText;
    public TMP_Text maxFuelText;
    public TMP_Text metalText;


    private void Start()
    {
        waterText.text = PlayerPrefs.GetFloat("water").ToString();
        foodText.text = PlayerPrefs.GetFloat("food").ToString();
        energyText.text = PlayerPrefs.GetFloat("energy").ToString();
        fuelText.text = PlayerPrefs.GetFloat("fuel").ToString();
        maxFuelText.text = PlayerPrefs.GetFloat("maxFuel").ToString();
        metalText.text = PlayerPrefs.GetFloat("metal").ToString();
    }

    public void UpdateWater() => waterText.text = PlayerPrefs.GetFloat("water").ToString();
    public void UpdateFood() => foodText.text = PlayerPrefs.GetFloat("food").ToString();
    public void UpdateEnergy() => energyText.text = PlayerPrefs.GetFloat("energy").ToString();
    public void UpdateFuel() => fuelText.text = PlayerPrefs.GetFloat("fuel").ToString();
    public void UpdateMaxFuel() => maxFuelText.text = PlayerPrefs.GetFloat("maxFuel").ToString();
    public void UpdateMetal() => metalText.text = PlayerPrefs.GetFloat("metal").ToString();
}