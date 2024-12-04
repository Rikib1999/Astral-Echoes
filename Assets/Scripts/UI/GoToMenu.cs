using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class GoToMenu : NetworkBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        NetworkManager.Singleton.Shutdown();
    }
}