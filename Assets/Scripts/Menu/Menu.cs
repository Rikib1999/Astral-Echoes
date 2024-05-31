using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string ip = "";
    public void OnPlayButton ()
    {
        SceneManager.LoadScene(1);
    }
    public void OnQuitButton ()
    {
        Application.Quit();
    }
    public void OnJoinButton ()
    {
        //Load multiplayer scene
    }
}
