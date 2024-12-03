using Assets.Scripts.Resources;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class FuelInMap : NetworkBehaviour
{
    public TMP_Text fuelText;
    public TMP_Text maxFuelText;

    private float fuel;

    private void Start()
    {
        fuel = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel);
        fuelText.text = fuel.ToString();
        maxFuelText.text = PlayerPrefs.GetFloat("maxFuel", ResourceDefaultValues.MaxFuel).ToString();
    }

    //public void UpdateFuel(float price) => PlayerPrefs.SetFloat("fuel", fuel - price);
}