using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonToSystem : NetworkBehaviour
{
    public void OnReturnButton()
    {
        if (!IsServer) return;

        var status = NetworkManager.SceneManager.LoadScene("SystemMap", LoadSceneMode.Single);

        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load return scene with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
}