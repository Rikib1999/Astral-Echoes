using Assets.Scripts;
using TMPro;
using UnityEngine;

public class SeedManager : Singleton<SeedManager>
{
    public void StringToSeedSave()
    {
        string seed = GameObject.FindGameObjectWithTag("SeedInput").GetComponent<TMP_InputField>().text;
        if (string.IsNullOrEmpty(seed)) seed = "122121";

        unchecked
        {
            int hash = 0;

            foreach (char c in seed) hash = (hash * 31) + c;

            if (PlayerPrefs.HasKey("seed")) return;

            PlayerPrefs.SetInt("seed", hash);
        }
    }
}