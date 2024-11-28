using UnityEngine;

public class WaterFoodDepletion : MonoBehaviour
{
    private float elapsedTime = 5;
    private float depletionRate = 100;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 10)
        {
            PlayerPrefs.SetFloat("water", PlayerPrefs.GetFloat("water") - depletionRate);
            PlayerPrefs.SetFloat("food", PlayerPrefs.GetFloat("food") - depletionRate);

            elapsedTime = 0;
        }
    }
}