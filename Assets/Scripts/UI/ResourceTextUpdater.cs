using Assets.Scripts.Resources;
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
        waterText.text = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water).ToString();
        foodText.text = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food).ToString();
        energyText.text = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy).ToString();
        fuelText.text = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel).ToString();
        maxFuelText.text = PlayerPrefs.GetFloat("maxFuel", ResourceDefaultValues.MaxFuel).ToString();
        metalText.text = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal).ToString();
    }

    public void UpdateWater() => waterText.text = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water).ToString();
    public void UpdateFood() => foodText.text = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food).ToString();
    public void UpdateEnergy() => energyText.text = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy).ToString();
    public void UpdateFuel() => fuelText.text = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel).ToString();
    public void UpdateMaxFuel() => maxFuelText.text = PlayerPrefs.GetFloat("maxFuel", ResourceDefaultValues.MaxFuel).ToString();
    public void UpdateMetal() => metalText.text = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal).ToString();
}