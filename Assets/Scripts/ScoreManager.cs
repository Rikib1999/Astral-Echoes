using UnityEngine;

namespace Assets.Scripts
{
    public static class ScoreManager
    {
        public static void AddResource(int amount)
        {
            int hs_res = PlayerPrefs.GetInt("hs_res", 0);
            int cs_res = PlayerPrefs.GetInt("cs_res", 0);

            cs_res += amount;

            PlayerPrefs.SetInt("cs_res", cs_res);
            if (cs_res > hs_res) PlayerPrefs.SetInt("hs_res", cs_res);
        }

        public static void IncrementKillCount()
        {
            int hs_enem = PlayerPrefs.GetInt("hs_enem", 0);
            int cs_enem = PlayerPrefs.GetInt("cs_enem", 0);

            cs_enem++;

            PlayerPrefs.SetInt("cs_enem", cs_enem);
            if (cs_enem > hs_enem) PlayerPrefs.SetInt("hs_enem", cs_enem);
        }

        public static void UpdateMaxDistance(int dist)
        {
            int hs_dist = PlayerPrefs.GetInt("hs_dist", 0);
            int cs_dist = PlayerPrefs.GetInt("cs_dist", 0);

            if (dist > cs_dist) PlayerPrefs.SetInt("cs_dist", dist);
            if (dist > hs_dist) PlayerPrefs.SetInt("hs_dist", dist);
        }
    }
}