using Assets.Scripts.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResourceTextUpdater : MonoBehaviour
{
    public TMP_Text waterText;
    public TMP_Text foodText;
    public TMP_Text energyText;
    public TMP_Text fuelText;
    public TMP_Text maxFuelText;
    public TMP_Text metalText;
    public TMP_Text ammoText;
    public Image rifleImage;
    public Image shotgunImage;

    public int ammoCount;

    private void Start()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        waterText.text = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water).ToString();
        foodText.text = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food).ToString();
        energyText.text = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy).ToString();
        if (fuelText != null) fuelText.text = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel).ToString();
        if (maxFuelText != null) maxFuelText.text = PlayerPrefs.GetFloat("maxFuel", ResourceDefaultValues.MaxFuel).ToString();
        metalText.text = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal).ToString();
        ammoCount = PlayerPrefs.GetInt("ammo", ResourceDefaultValues.Ammo);
        if (ammoText != null) ammoText.text = ammoCount.ToString();
        if (rifleImage != null) rifleImage.color = new Color(rifleImage.color.r, rifleImage.color.g, rifleImage.color.b, PlayerPrefs.GetInt("rifle", 0));
        if (shotgunImage != null) shotgunImage.color = new Color(shotgunImage.color.r, shotgunImage.color.g, shotgunImage.color.b, PlayerPrefs.GetInt("shotgun", 0));
    }

    private void OnSceneUnloaded(Scene current)
    {
        SaveAmmo();
    }

    public void SetWater(float amount) => waterText.text = amount.ToString();
    public void SetFood(float amount) => foodText.text = amount.ToString();
    public void SetEnergy(float amount) => energyText.text = amount.ToString();
    public void SetMetal(float amount) => metalText.text = amount.ToString();
    public void SetFuel(float amount) => fuelText.text = amount.ToString();
    public void SetAmmo(float amount) => ammoText.text = amount.ToString();
    public void SetRifle(int value) => rifleImage.color = new Color(rifleImage.color.r, rifleImage.color.g, rifleImage.color.b, value);
    public void SetShotgun(int value) => shotgunImage.color = new Color(shotgunImage.color.r, shotgunImage.color.g, shotgunImage.color.b, value);

    public void UpdateWater() => waterText.text = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water).ToString();
    public void UpdateFood() => foodText.text = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food).ToString();
    public void UpdateEnergy() => energyText.text = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy).ToString();
    public void UpdateFuel() => fuelText.text = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel).ToString();
    public void UpdateMaxFuel() => maxFuelText.text = PlayerPrefs.GetFloat("maxFuel", ResourceDefaultValues.MaxFuel).ToString();
    public void UpdateMetal() => metalText.text = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal).ToString();

    public void SaveWater()
    {
        PlayerPrefs.SetFloat("water", float.TryParse(waterText.text, out float x) ? x : ResourceDefaultValues.Water);
    }

    public void SaveFood()
    {
        PlayerPrefs.SetFloat("food", float.TryParse(foodText.text, out float x) ? x : ResourceDefaultValues.Food);
    }

    public void SaveEnergy()
    {
        PlayerPrefs.SetFloat("energy", float.TryParse(energyText.text, out float x) ? x : ResourceDefaultValues.Energy);
    }

    public void SaveMetal()
    {
        PlayerPrefs.SetFloat("metal", float.TryParse(metalText.text, out float x) ? x : ResourceDefaultValues.Metal);
    }
    
    public void SaveAmmo()
    {
        PlayerPrefs.SetInt("ammo", int.TryParse(ammoText.text, out int x) ? x : ResourceDefaultValues.Ammo);
    }
}