using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GoToMenu : NetworkBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        NetworkManager.Singleton.Shutdown();
    }
}