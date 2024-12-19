using TMPro;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    public TMP_Text highscores;
    public TMP_Text currentscores;

    void OnEnable()
    {
        int hs_dist = PlayerPrefs.GetInt("hs_dist", 0);
        int hs_enem = PlayerPrefs.GetInt("hs_enem", 0);
        int hs_res = PlayerPrefs.GetInt("hs_res", 0);

        int cs_dist = PlayerPrefs.GetInt("cs_dist", 0);
        int cs_enem = PlayerPrefs.GetInt("cs_enem", 0);
        int cs_res = PlayerPrefs.GetInt("cs_res", 0);

        highscores.text = "HIGHSCORE:\n\nMax distance traveled:\n" + hs_dist + "\n\nEnemies killed:\n" + hs_enem + "\n\nResources gathered:\n" + hs_res;
        currentscores.text = "CURRENT SCORE:\n\nMax distance traveled:\n" + cs_dist + "\n\nEnemies killed:\n" + cs_enem + "\n\nResources gathered:\n" + cs_res;
    }
}