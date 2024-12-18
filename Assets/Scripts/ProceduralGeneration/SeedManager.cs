using Assets.Scripts;
using TMPro;
using UnityEngine;

public class SeedManager : Singleton<SeedManager>
{
    public TMP_Text seedText;

    public void StringToSeedSave()
    {
        string seed = seedText.text;
        if (string.IsNullOrEmpty(seed)) seed = "12345";

        unchecked
        {
            int hash = 0;

            foreach (char c in seed) hash = (hash * 31) + c;

            if (PlayerPrefs.HasKey("seed")) return;

            PlayerPrefs.SetInt("seed", hash);
        }
    }
}