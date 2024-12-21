using Assets.Scripts;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GoToMenu : NetworkBehaviour
{
    public void GoToMainMenu()
    {
        if (PlanetMapManager.Instance != null) PlanetMapManager.Instance.DestroyInstance();
        if (SystemMapManager.Instance != null) SystemMapManager.Instance.DestroyInstance();
        SceneManager.LoadScene("Menu");
        NetworkManager.Singleton.Shutdown();
    }
}