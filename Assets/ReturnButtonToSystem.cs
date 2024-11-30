using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonToSystem : NetworkBehaviour
{
    [SerializeField] public UnityEditor.SceneAsset return_scene;

    public void OnReturnButton()
    {
        if (!IsServer) return;

        var status = NetworkManager.SceneManager.LoadScene(return_scene.name,LoadSceneMode.Single);

        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load return scene with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
}