using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenuOnStart : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Menu");
    }
}