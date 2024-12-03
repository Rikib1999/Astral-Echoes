using Assets.Scripts.Resources;
using TMPro;
using UnityEngine;

public class WaterFoodDepletion : MonoBehaviour
{
    private float elapsedTime = 5;
    private float depletionRate = 10;

    public TMP_Text waterText;
    public TMP_Text foodText;

    private float water;
    private float food;

    private void Start()
    {
        water = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water);
        food = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food);

        waterText.text = water.ToString();
        foodText.text = food.ToString();

        PlayerPrefs.SetFloat("water", water);
        PlayerPrefs.SetFloat("food", food);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 10)
        {
            water = PlayerPrefs.GetFloat("water");
            food = PlayerPrefs.GetFloat("food");

            if (float.TryParse(waterText.text, out float currentWater)) water = currentWater;
            if (float.TryParse(foodText.text, out float currentFood)) food = currentFood;

            water -= depletionRate;
            food -= depletionRate;

            waterText.text = water.ToString();
            foodText.text = food.ToString();

            PlayerPrefs.SetFloat("water", water);
            PlayerPrefs.SetFloat("food", food);

            elapsedTime = 0;
        }
    }
}